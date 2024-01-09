using Entities.Exceptions;

namespace Entities;

public class StageNotEmptyBadRequestException : BadRequestException
{
  public StageNotEmptyBadRequestException() : base("There are tasks in this stage. Unable to delete.")
  {
  }
}
