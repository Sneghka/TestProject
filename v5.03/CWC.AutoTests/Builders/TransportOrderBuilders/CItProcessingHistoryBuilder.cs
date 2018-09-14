using Cwc.BaseData;
using Cwc.Common;
using Cwc.Transport;
using Cwc.Transport.Model;
using CWC.AutoTests.Model;
using System;
using System.Linq;

namespace CWC.AutoTests.ObjectBuilder
{
    public class CitProcessingHistoryBuilder
    {
        DataBaseParams _dbParams;
        CitProcessingHistory entity;
        UserParams userParams;
        public CitProcessingHistoryBuilder()
        {
            _dbParams = new DataBaseParams();
        }

        public CitProcessingHistoryBuilder With_ProcessName(ProcessName value)
        {
            if (entity != null)
            {
                entity.ProcessName = value;

                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public CitProcessingHistoryBuilder With_ProcessPhase(ProcessPhase? value)
        {
            if (entity != null)
            {
                entity.ProcessPhase = value;

                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public CitProcessingHistoryBuilder With_Status(Int32 value)
        {
            if (entity != null)
            {
                entity.Status = value;

                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public CitProcessingHistoryBuilder With_IsWithException(Boolean? value)
        {
            if (entity != null)
            {
                entity.IsWithException = value;

                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public CitProcessingHistoryBuilder With_WorkstationID(Int32? value)
        {
            if (entity != null)
            {
                entity.WorkstationID = value;

                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public CitProcessingHistoryBuilder With_ObjectID(int value)
        {
            if (entity != null)
            {
                entity.ObjectID = value;

                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public CitProcessingHistoryBuilder With_DateCreated(DateTime value)
        {
            if (entity != null)
            {
                entity.DateCreated = value;

                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public CitProcessingHistoryBuilder With_AuthorID(Int32 value)
        {
            if (entity != null)
            {
                entity.AuthorID = value;

                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public CitProcessingHistoryBuilder With_ObjectClassID(Int32? value)
        {

            if (entity != null)
            {
                entity.ObjectClassID = value.Value;

                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public CitProcessingHistoryBuilder With_ID(int value)
        {
            if (entity != null)
            {
                entity.ID = value;

                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public CitProcessingHistoryBuilder With_UserParams(UserParams value)
        {
            userParams = value;
            return this;
        }

        public CitProcessingHistoryBuilder New()
        {
            entity = new CitProcessingHistory();

            return this;
        }

        public static implicit operator CitProcessingHistory(CitProcessingHistoryBuilder ins)
        {
            return ins.Build();
        }

        public CitProcessingHistory Build()
        {
            return entity;
        }

        public CitProcessingHistoryBuilder SaveToDb()
        {
            var temp = entity;

            var result = TransportFacade.CitProcessingHistoryService.Save(temp, userParams);

            if (!result.IsSuccess)
            {
                throw new InvalidOperationException($"Cit processing history exception saving failed. Rreason: {result.GetMessage()}");
            }

            return this;
        }

        public CitProcessingHistoryBuilder Take(Func<CitProcessingHistory, bool> expression)
        {
            using (var context = new AutomationTransportDataContext())
            {
                entity = context.CitProcessingHistories.FirstOrDefault(expression);

                if (entity == null)
                {
                    throw new ArgumentNullException("Cit Processing History by provided criteria wasn't found");
                }
            }

            return this;
        }
    }
}