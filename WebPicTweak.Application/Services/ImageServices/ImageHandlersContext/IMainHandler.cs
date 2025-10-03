using WebPicTweak.Application.Transactions;

namespace WebPicTweak.Application.Services.ImageServices.ImageHandlersContext
{
    public interface IMainHandler
    {
        Task Processing(JpegJobDTO jobDTO);
    }
}