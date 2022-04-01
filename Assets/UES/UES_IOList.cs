using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


/// <summary>
/// IOList for UES_BaseModules' Trigger and Power connections.
/// </summary>
[Serializable]
    public class UES_IOList:ARX_IOList<UES_BaseModule>
    {

    public UES_IOList(UES_BaseModule module) : base(module) { }
    protected override List<UES_BaseModule> GetInputList(UES_BaseModule other)
    {
        return Inputs;
    }

    protected override List<UES_BaseModule> GetOutputList(UES_BaseModule other)
    {
        return Outputs;
    }

    public override UES_BaseModule GetNullModule()
    {
        return null;
    }

}

