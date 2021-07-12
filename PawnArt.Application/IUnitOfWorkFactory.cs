using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PawnArt.Application
{
	/// <summary>
	/// Creates units of work.
	/// </summary>
	public interface IUnitOfWorkFactory
	{
		/// <summary>
		/// Creates and starts a new <see cref="IUnitOfWork"/>.
		/// </summary>
		/// <returns>The created <see cref="IUnitOfWork"/>.</returns>
		Task<IUnitOfWork> CreateAsync(CancellationToken cancellationToken = default);
	}
}
