using ForumsService.Dtos;

namespace ForumsService.SyncDataServices.Http
{
    public interface ICommentDataClient
    {
        Task SendForumToComment(ForumReadDto forum);
    }
}