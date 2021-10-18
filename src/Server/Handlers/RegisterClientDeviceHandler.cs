using MediatR;
using Microsoft.EntityFrameworkCore;
using Nova.Identity.Data;
using Nova.Identity.Entities;
using Nova.Identity.Processes;
using Nova.Identity.Requests;
using Nova.Identity.Responses;

namespace Nova.Identity.Handlers
{
    sealed class RegisterClientDeviceHandler : IRequestHandler<RegisterClientDeviceRequest, RegisterClientDeviceResponse>
    {
        private readonly IDbContextFactory<IdentityDbContext> _contextFactory;
        private readonly InsertClientDevice _insertClientDevice;
        private readonly UpdateClientDevice _updateClientDevice;

        public RegisterClientDeviceHandler(IDbContextFactory<IdentityDbContext> contextFactory, InsertClientDevice insertClientDevice, UpdateClientDevice updateClientDevice)
        {
            _contextFactory = contextFactory;
            _insertClientDevice = insertClientDevice;
            _updateClientDevice = updateClientDevice;
        }

        public async Task<RegisterClientDeviceResponse> Handle(RegisterClientDeviceRequest request, CancellationToken cancellationToken)
        {
            using var context = _contextFactory.CreateDbContext();
            using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);

            var clientDevice =
                await context.GetClientDeviceAsync(request.Id ?? 0, cancellationToken) ?? 
                await context.ClientDevices.SingleOrDefaultAsync(clientDevice => clientDevice.IpAddress == request.IpAddress, cancellationToken) ?? 
                new()
                {
                    IpAddress = request.IpAddress,
                    Name = request.Name ?? request.IpAddress
                };
            
            if (clientDevice.Id == 0)
            {
                clientDevice = await _insertClientDevice.ExecuteAsync
                (
                    context, 
                    clientDevice,
                    cancellationToken
                );   
            }
            else if (HasChanges(clientDevice, request))
            {
                clientDevice = await _updateClientDevice.ExecuteAsync
                (
                    context,
                    clientDevice with 
                    {
                        IpAddress = request.IpAddress,
                        Name = request.Name ?? request.IpAddress
                    },
                    cancellationToken
                );
            }

            await context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            return new()
            {
                Id = clientDevice.Id,
                IpAddress = clientDevice.IpAddress,
                Name = clientDevice.Name
            };
        }

        static bool HasChanges(ClientDevice clientDevice, RegisterClientDeviceRequest request) => 
            clientDevice.IpAddress != request.IpAddress ||
            clientDevice.Name != (request.Name ?? request.IpAddress);
    }
}