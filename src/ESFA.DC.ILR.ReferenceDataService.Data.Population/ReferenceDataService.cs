using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.Model.Interface;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Interface;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population
{
    public class ReferenceDataService<TOut, TMapOut> : IReferenceDataService<TOut, TMapOut>
    {
        private IMessageMapper<TMapOut> _messageMapper;
        private IRetrievalService<TOut, TMapOut> _retrievalService;

        public ReferenceDataService(IMessageMapper<TMapOut> messageMapper, IRetrievalService<TOut, TMapOut> retrievalService)
        {
            _messageMapper = messageMapper;
            _retrievalService = retrievalService;
        }

        public Task<TOut> Retrieve(IMessage message, CancellationToken cancellationToken)
        {
            var input = _messageMapper.MapFromMessage(message);

            return _retrievalService.RetrieveAsync(input, cancellationToken);
        }
    }
}
