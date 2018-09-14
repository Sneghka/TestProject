using Cwc.CallManagement;
using Cwc.Contracts;
using CWC.AutoTests.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;

namespace Specflow.Automation.Backend.Hooks
{
    [Binding, Scope(Tag = "callmanagement-data-generation-required")]
    public sealed class CallManagementDataConfigurationHooks
    {
        private const string UrgentCallCategoryCode = "Urgent";
        private const string NormalCallCategoryCode = "Normal";
        private const string LowCallCategoryCode = "Low";
        private const string SolutionCodeNumber1 = "001";
        private const string SolutionCodeNumber2 = "002";

        public static CallCategory UrgentCallCategory { get; private set; }
        public static CallCategory NormalCallCategory { get; private set; }
        public static CallCategory LowCallCategory { get; private set; }
        public static SolutionCode SolutionCode1 { get; private set; }
        public static SolutionCode SolutionCode2 { get; private set; }

        [BeforeFeature(Order = 1)]
        public static void ConfigureCallCategories()
        {
            using (var context = new AutomationCallManagementDataContext())
            {
                try
                {
                    UrgentCallCategory = context.CallCategories.AsNoTracking().FirstOrDefault(x => x.Name == UrgentCallCategoryCode);
                    if (UrgentCallCategory == null)
                    {
                        UrgentCallCategory = new CallCategory
                        {
                            Name = UrgentCallCategoryCode,
                            Description = UrgentCallCategoryCode,
                            Type = CallCategoryType.Incident
                        };

                        context.CallCategories.Add(UrgentCallCategory);
                    }

                    NormalCallCategory = context.CallCategories.AsNoTracking().FirstOrDefault(x => x.Name == NormalCallCategoryCode);
                    if (NormalCallCategory == null)
                    {
                        NormalCallCategory = new CallCategory
                        {
                            Name = NormalCallCategoryCode,
                            Description = NormalCallCategoryCode,
                            Type = CallCategoryType.Incident
                        };

                        context.CallCategories.Add(NormalCallCategory);
                    }

                    LowCallCategory = context.CallCategories.AsNoTracking().FirstOrDefault(x => x.Name == LowCallCategoryCode);
                    if (LowCallCategory == null)
                    {
                        LowCallCategory = new CallCategory
                        {
                            Name = LowCallCategoryCode,
                            Description = LowCallCategoryCode,
                            Type = CallCategoryType.Incident
                        };

                        context.CallCategories.Add(LowCallCategory);
                    }

                    context.SaveChangesAsync();
                }
                catch
                {
                    throw;
                }
            }
        }

        [BeforeFeature(Order = 2)]
        public static void ConfigureSolutionCodes()
        {
            using (var context = new AutomationCallManagementDataContext())
            {
                try
                {
                    SolutionCode1 = context.SolutionCodes.AsNoTracking().FirstOrDefault(x => x.Code == SolutionCodeNumber1);
                    if (SolutionCode1 == null)
                    {
                        SolutionCode1 = new SolutionCode
                        {
                            Code = SolutionCodeNumber1,
                            Type = SolutionCodeType.Solution,
                            Description = SolutionCodeNumber1
                        };

                        context.SolutionCodes.Add(SolutionCode1);
                    }

                    SolutionCode2 = context.SolutionCodes.AsNoTracking().FirstOrDefault(x => x.Code == SolutionCodeNumber2);
                    if (SolutionCode2 == null)
                    {
                        SolutionCode2 = new SolutionCode
                        {
                            Code = SolutionCodeNumber2,
                            Type = SolutionCodeType.Solution,
                            Description = SolutionCodeNumber2
                        };

                        context.SolutionCodes.Add(SolutionCode2);
                    }

                    context.SaveChangesAsync();
                }
                catch
                {
                    throw;
                }
            }
        }
    }
}
