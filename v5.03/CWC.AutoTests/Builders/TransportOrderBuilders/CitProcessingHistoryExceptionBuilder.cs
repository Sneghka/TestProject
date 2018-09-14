using Cwc.Common;
using Cwc.Transport;
using Cwc.Transport.Enums;
using Cwc.Transport.Model;
using CWC.AutoTests.Model;
using System;
using System.Linq;

namespace CWC.AutoTests.ObjectBuilder
{
    public class CitProcessingHistoryExceptionBuilder
    {
        DataBaseParams _dbParams;
        CitProcessingHistoryException entity;

        public CitProcessingHistoryExceptionBuilder()
        {
            _dbParams = new DataBaseParams();
        }

        public CitProcessingHistoryExceptionBuilder With_Status(ProcessingHistoryExceptionStatus value)
        {
            if (entity != null)
            {
                entity.Status = value;

                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public CitProcessingHistoryExceptionBuilder With_Remark(String value)
        {
            if (entity != null)
            {
                entity.Remark = value;

                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public CitProcessingHistoryExceptionBuilder With_DateResolved(DateTime? value)
        {
            if (entity != null)
            {
                entity.DateResolved = value;

                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public CitProcessingHistoryExceptionBuilder With_Action(ExceptionAction? value)
        {
            if (entity != null)
            {
                entity.Action = value;

                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public CitProcessingHistoryExceptionBuilder With_Exception(long value)
        {
            if (entity != null)
            {
                entity.ExceptionCaseID = (int)value;

                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public CitProcessingHistoryExceptionBuilder With_Exception(ExceptionCase value)
        {
            if (entity != null)
            {
                entity.ExceptionCase = value;

                this.With_Exception(entity.ExceptionCase.ID);

                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public CitProcessingHistoryExceptionBuilder With_CitProcessingHistoryID(int value)
        {
            if (entity != null)
            {
                entity.CitProcessingHistoryID = value;

                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public CitProcessingHistoryExceptionBuilder With_SiteID(Int32 value)
        {
            if (entity != null)
            {
                entity.SiteID = value;

                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public CitProcessingHistoryExceptionBuilder With_WorkstationID(Int32? value)
        {
            if (entity != null)
            {
                entity.WorkstationID = value;

                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public CitProcessingHistoryExceptionBuilder With_ResolvedByID(Int32? value)
        {
            if (entity != null)
            {
                entity.ResolvedByID = value;

                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public CitProcessingHistoryExceptionBuilder With_SecondUserID(Int32? value)
        {
            if (entity != null)
            {
                entity.SecondUserID = value;

                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public CitProcessingHistoryExceptionBuilder With_ReasonCodeID(Int32? value)
        {
            if (entity != null)
            {
                entity.ReasonCodeID = value;
                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public CitProcessingHistoryExceptionBuilder With_ID(Int32 value)
        {
            if (entity != null)
            {
                entity.ID = value;

                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }


        public CitProcessingHistoryExceptionBuilder New()
        {
            entity = new CitProcessingHistoryException();

            return this;
        }

        public static implicit operator CitProcessingHistoryException(CitProcessingHistoryExceptionBuilder ins)
        {
            return ins.Build();
        }

        public CitProcessingHistoryException Build()
        {
            return entity;
        }

        public CitProcessingHistoryExceptionBuilder SaveToDb()
        {
            var temp = entity;

            var result = TransportFacade.CitProcessingHistoryExceptionService.Save(temp);

            if (!result.IsSuccess)
            {
                throw new InvalidOperationException($"Cit processing history exception saving failed. Reason: {result.GetMessage()}");
            }

            return this;
        }

        public CitProcessingHistoryExceptionBuilder Take(Func<CitProcessingHistoryException, bool> expression)
        {
            using (var context = new AutomationTransportDataContext())
            {
                entity = context.CitProcessingHistoryExceptions.FirstOrDefault(expression);

                if (entity == null)
                {
                    throw new ArgumentNullException("Cit Processing History Exception by provided criteria wasn't found");
                }
            }

            return this;
        }
    }
}