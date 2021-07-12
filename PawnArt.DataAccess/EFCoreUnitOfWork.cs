using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore.Storage;

using PawnArt.Application;
using PawnArt.Logic.DataAccessInterface;
namespace PawnArt.DataAccess
{
	internal class EFCoreUnitOfWork : IUnitOfWork
	{
		private readonly GameContext dbContext;
		private readonly IDbContextTransaction transaction;

		/// <summary>
		/// Initializes a new instance of the <see cref="EFCoreUnitOfWork"/> class.
		/// </summary>
		/// <param name="dbContext">The database context.</param>
		/// <param name="transaction">A transaction.</param>
		public EFCoreUnitOfWork(GameContext dbContext, IDbContextTransaction transaction)
		{
			this.dbContext = dbContext;
			this.transaction = transaction;
		}

		/// <summary>
		/// Commits all the changes to the database.
		/// </summary>
		/// <param name="cancellationToken">A token for canceling the operation.</param>
		public async Task CompleteAsync(CancellationToken cancellationToken = default)
		{
			await dbContext.SaveChangesAsync(cancellationToken);
			await transaction.CommitAsync(cancellationToken);
		}

		/// <inheritdoc/>
		public ValueTask DisposeAsync()
		{
			return transaction.DisposeAsync();
		}
	}
}
