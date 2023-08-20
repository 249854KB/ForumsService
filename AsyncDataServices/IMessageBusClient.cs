using ForumsService.Dtos;

namespace ForumsService.AsyncDataServices
{
    public interface IMessageBusClient
    {
        void PublishNewForum(ForumPublishedDto forumPublishedDto);
    }
}