using CORESI.WPF.Core;
using CORESI.WPF.Model;
using EXGEPA.Output.Controls;

namespace EXGEPA.Output
{

    public sealed class Module : AModule
    {

        public override int Priority
        {
            get { return 5; }
        }
        public override void AddGroups()

        {
            var outputGroup = new Group("Sorties");

            outputGroup.AddCommand("Proposés à la reforme", IconProvider.Reforme, AddPage<OutputView, ProposeToReformViewModel>, true);
            outputGroup.AddCommand("Reforme", IconProvider.Reforme, AddPage<OutputView, ReformeCertificateViewModel>, true);
            outputGroup.AddCommand("Vente", IconProvider.Download, AddPage<OutputView, SaleCertificateViewModel>, true);
            outputGroup.AddCommand("Cession", IconProvider.CessionSmall, AddPage<OutputView, CessionCertificateViewModel>, true);
            outputGroup.AddCommand("Disparition", IconProvider.DisparitionSmall, AddPage<OutputView, DisparitionCertificateViewModel>, true);
            outputGroup.AddCommand("Destruction", IconProvider.DestructionSmall, AddPage<OutputView, DestructionCertificateViewModel>, true);

            uIService.AddGroupToHomePage(outputGroup);
        }
    }
}
