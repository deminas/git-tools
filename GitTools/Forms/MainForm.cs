using DevExpress.Mvvm.POCO;
using DevExpress.XtraBars.FluentDesignSystem;
using DevExpress.XtraBars.Navigation;
using DevExpress.XtraEditors;
using GitTools.Core;
using Microsoft.Extensions.DependencyInjection;

namespace GitTools.Forms
{
    internal partial class MainForm : FluentDesignForm
    {
        private readonly MainCore _core;
        private readonly IServiceCollection _services;

        public MainForm(MainCore core, IServiceCollection services)
        {
            _core = core;
            _services = services;

            InitializeComponent();

            RefreshRepositories();
        }

        private void RepositoriesMenuItem_Click(object sender, EventArgs e)
        {
            var form = _services.GetService<RepositoriesForm>();
            form.ShowDialog();
        }

        public void RefreshRepositories()
        {
            RepositoriesMenuItem.Elements.Clear();

            foreach (var repository in _core.Repositories)
            {
                RepositoriesMenuItem.Elements.Add(new AccordionControlElement
                {
                    Style = ElementStyle.Item,
                    Text = repository.Name,
                    Tag = repository
                });
            }
        }
    }
}
