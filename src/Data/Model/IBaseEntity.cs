namespace DotNetBoilerplate.Data.Entity
{
  using System;

  /// <summary>
  /// Defines a base node object
  /// </summary>
  public interface IBaseEntity
  {
    long Id { get; set; }

    Guid ExternalId { get; set; }

    bool IsDeleted { get; set; }

    long CreatedTicketId { get; set; }

    long ModifiedTicketId { get; set; }
  }
}