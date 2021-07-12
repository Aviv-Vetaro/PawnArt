using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using PawnArt.Application;

namespace PawnArt.DataAccess
{
	internal class EFCoreUnitOfWorkFactory : IUnitOfWorkFactory
	{
		private readonly GameContext dbContext;

		/// <summary>
		/// Initializes a new instance of the <see cref="EFCoreUnitOfWorkFactory"/> class.
		/// </summary>
		/// <param name="dbContext">The database context.</param>
		public EFCoreUnitOfWorkFactory(GameContext dbContext)
		{
			this.dbContext = dbContext;
		}

		/// <inheritdoc/>
		public async Task<IUnitOfWork> CreateAsync(CancellationToken cancellationToken = default)
		{
			var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
			return new EFCoreUnitOfWork(dbContext, transaction);
		}
	}
}
