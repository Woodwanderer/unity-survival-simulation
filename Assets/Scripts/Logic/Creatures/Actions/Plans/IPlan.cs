using System.Collections.Generic;
public interface IPlan
{
    IEnumerable<IAction> Build();
}
