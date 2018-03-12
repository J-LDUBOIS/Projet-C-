using Core1.Persistance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Core1.Services
{
    public abstract class ParentClass

    {
        public SampleDatabaseEntities connexion;


            public ParentClass ()
            {
            connexion = new SampleDatabaseEntities();
            }
    }
}
