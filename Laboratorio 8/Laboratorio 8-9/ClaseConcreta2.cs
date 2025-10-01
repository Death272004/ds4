using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class ClaseConcreta2 : ClaseAbstracta
{
    protected override string tomarValor()
    {
        return "ClaseConcreta2";
    }
    public override string prefixValor(string prefix)
    {
        return $"{prefix}ClaseConcreta2";
    }
}
