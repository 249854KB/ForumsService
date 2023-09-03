using System;
using System.Collections.Generic;
using AutoMapper;
using ForumsService.AsyncDataServices;
using ForumsService.Data;
using ForumsService.Dtos;
using ForumsService.Models;
using Microsoft.AspNetCore.Mvc;

namespace ForumsService.Controllers
{
    [Route("api/f")]
    [ApiController]
    public class ForumsController : ControllerBase
    {
        private readonly IForumRepo _repository;
        private readonly IMapper _mapper;
        private readonly IMessageBusClient _messageBusClient;

        public ForumsController(IForumRepo repository, IMapper mapper, IMessageBusClient messageBusClient)
        {
            _repository = repository;
            _mapper = mapper;
            _messageBusClient = messageBusClient;
        }

        [HttpGet("users/{userId}/[controller]", Name = "GetForumsForUser")]
        public ActionResult<IEnumerable<ForumReadDto>> GetForumsForUser(int userId)
        {
            Console.WriteLine($"--> Hit GetForumsForUser: {userId}");

            if (!_repository.UserExists(userId))
            {
                return NotFound();
            }

            var forums = _repository.GetForumsForUser(userId);

            return Ok(_mapper.Map<IEnumerable<ForumReadDto>>(forums));
        }

         [HttpGet("[controller]", Name = "GetAllForums")]
        public ActionResult<IEnumerable<ForumReadDto>> GetAllForums()
        {
            Console.WriteLine($"--> Hit GetAllForums");


            var forums = _repository.GetAllForums();
            if(forums.Count() >0)
            {
                return Ok(_mapper.Map<IEnumerable<ForumReadDto>>(forums));
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet("users/{userId}/[controller]/{forumId}", Name = "GetForumForUser")]
        public ActionResult<ForumReadDto> GetForumForUser(int userId, int forumId)
        {
            Console.WriteLine($"--> Hit GetForumForUser: {userId} / {forumId}");

            if (!_repository.UserExists(userId))
            {
                return NotFound();
            }

            var forum = _repository.GetForum(userId, forumId);

            if(forum == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<ForumReadDto>(forum));
        }

        [HttpPost("users/{userId}/[controller]", Name ="CreateForumForUser")]
        public ActionResult<ForumReadDto> CreateForumForUser(int userId, ForumCreateDto forumDto)
        {
             Console.WriteLine($"--> Hit CreateForumForUser: {userId}");

            if (!_repository.UserExists(userId))
            {
                return NotFound();
            }

            var forum = _mapper.Map<Forum>(forumDto);

            _repository.CreateForum(userId, forum);
            _repository.SaveChanges();

            var forumReadDto = _mapper.Map<ForumReadDto>(forum);

            try
            {
                var forumPublishedDto = _mapper.Map<ForumPublishedDto>(forumReadDto);
                forumPublishedDto.Event = "Forum_Published";
                _messageBusClient.PublishNewForum(forumPublishedDto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Asynchro Error: {ex.Message}");
            }

            return CreatedAtRoute(nameof(GetForumForUser),
                new {userId = userId, forumId = forumReadDto.Id}, forumReadDto);
        }

    }
}