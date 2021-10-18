using CodeCompanion.EntityFrameworkCore;
using CodeCompanion.Processes;
using Microsoft.EntityFrameworkCore;
using Nova.Identity.Data;
using Nova.Identity.Entities;
using Nova.Identity.Exceptions;

namespace Nova.Identity.Processes
{
    sealed class UpdateClientDevice : IAsyncProcess<ClientDevice, ClientDevice>
    {
        public async Task<ClientDevice> ExecuteAsync(IProcessContext processContext, ClientDevice clientDevice, CancellationToken cancellationToken = default)
        {
            if (processContext is IdentityDbContext context)
            {
                if (await IpAddressExistsAsync(context, clientDevice.Id, clientDevice.IpAddress, cancellationToken))
                    throw new ClientDeviceIpAddressAlreadyExistsException { IpAddress = clientDevice.IpAddress };

                if (await NameExistsAsync(context, clientDevice.Id, clientDevice.Name, cancellationToken))
                    throw new ClientDeviceNameAlreadyExistsException { Name = clientDevice.Name };

                context.ClientDevices.Update(clientDevice, context.CurrentFootprint);
                return clientDevice;
            }

            throw InvalidProcessContextException.Expects<IdentityDbContext>();
        }

        static async Task<bool> IpAddressExistsAsync(IdentityDbContext context, long id, string ipAddress, CancellationToken cancellationToken) => await context.ClientDevices
            .Where(clientDevice => clientDevice.Id != id)
            .Where(clientDevice => clientDevice.IpAddress == ipAddress)
            .AnyAsync(cancellationToken);

        static async Task<bool> NameExistsAsync(IdentityDbContext context, long id, string name, CancellationToken cancellationToken) => await context.ClientDevices
            .Where(clientDevice => clientDevice.Id != id)
            .Where(clientDevice => clientDevice.Name == name)
            .AnyAsync(cancellationToken);
    }
}