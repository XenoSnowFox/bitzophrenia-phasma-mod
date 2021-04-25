using MelonLoader;
using UnityEngine;
using Object = Il2CppSystem.Object;
using Boolean = Il2CppSystem.Boolean;
using Int32 = Il2CppSystem.Int32;

namespace Bitzophrenia {
    namespace Phasma {
        public class RPC {

            public static Object[] GetObject(int i, bool isTrue = true, int rangeMin = 0, int rangeMax = 0, bool rangeFirst = false, bool isPosition = false, Vector3 pos = new Vector3()) {
                MelonLogger.Msg("getRPCObject");
                Object[] obj = new Object[i];
                if (i > 0)
                {
                    Boolean boolean = default(Boolean);
                    if (!rangeFirst)
                    {
                        if (isTrue)
                            boolean.m_value = true;
                        else
                            boolean.m_value = false;
                        obj[0] = boolean.BoxIl2CppObject();

                        if (i == 2)
                        {
                            Int32 integer = default(Int32);
                            integer.m_value = Random.Range(rangeMin, rangeMax);
                            obj[1] = integer.BoxIl2CppObject();
                        }
                    }
                    else
                    {
                        Int32 integer = default(Int32);
                        integer.m_value = Random.Range(rangeMin, rangeMax);
                        obj[0] = integer.BoxIl2CppObject();
                    }
                }
                if (isPosition)
                {
                    Vector3 vector = default(Vector3);
                    vector = pos;
                    obj[0] = vector.BoxIl2CppObject();
                }

                return obj;
            }

            public static Object[] getObjectEmf(int i, Vector3 pos, int type = 0)
            {
                MelonLogger.Msg("getRPCObjectEmf");
                Object[] obj = new Object[i];
                if (i > 0)
                {
                    Vector3 vector = default(Vector3);
                    Int32 integer = default(Int32);
                    vector = pos;
                    integer.m_value = type;

                    obj[0] = vector.BoxIl2CppObject();
                    obj[1] = integer.BoxIl2CppObject();
                }
                return obj;
            }
        }
    }
}