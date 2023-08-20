using System.Threading.Tasks;
using AutoMapper;
using Grpc.Core;
using ForumsService.Data;

namespace ForumsService.SyncDataServices.Grpc
{
    public class GrpcForumService : GrpcForum.GrpcForumBase
    {
        private readonly IForumRepo _repository;
        private readonly IMapper _mapper;

        public GrpcForumService(IForumRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public override Task<ForumResponse> GetAllForums(GetAllRequestForum request, ServerCallContext context)
        {
            var response = new ForumResponse();
            var forums = _repository.GetAllForums();

            foreach(var plat in forums)
            {
                response.Forum.Add(_mapper.Map<GrpcForumModel>(plat));
            }

            return Task.FromResult(response);
        }
    }
}