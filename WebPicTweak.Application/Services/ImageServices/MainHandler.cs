using WebPicTweak.Application.Services.ImageServices.ImageHandlersContext;
using WebPicTweak.Application.Transactions;
using WebPicTweak.Core.Abstractions.Image;
using WebPicTweak.Core.Const;

namespace WebPicTweak.Application.Services.ImageServices
{
    public class MainHandler : IMainHandler
    {
        private readonly IImageHandlerFactory _imageHandlerFactory;

        public MainHandler(IImageHandlerFactory imageHandlerFactory)
        {
            _imageHandlerFactory = imageHandlerFactory;
        }
        public async Task Processing(JpegJobDTO jobDTO)
        {
            var handlers = new List<IImageHandlerAsync>();

            if (jobDTO.Options.SizeScale != SizeScale.None)
                handlers.Add(_imageHandlerFactory.CreateResize(jobDTO.Options.SizeScale));

            if (jobDTO.Options.Compress != CompressLevel.None)
                handlers.Add(_imageHandlerFactory.CreateCompress(jobDTO.Options.Compress));

            if (jobDTO.Options.RemoveEXIF)
                handlers.Add(_imageHandlerFactory.CreateRemoveExif());

            foreach (IImageHandlerAsync handler in handlers)
            {
                byte[] newData = await handler.Handler(jobDTO.FilePath);
                File.WriteAllBytes(jobDTO.FilePath, newData);
            }
            handlers.Clear();
        }
    }
}
