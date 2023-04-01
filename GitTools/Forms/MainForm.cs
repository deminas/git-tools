using DevExpress.XtraBars.FluentDesignSystem;
using DevExpress.XtraBars.Navigation;
using GitTools.Core;

namespace GitTools.Forms
{
    internal partial class MainForm : FluentDesignForm
    {
        private readonly MainCore _core;

        public MainForm(MainCore core)
        {
            _core = core;

            InitializeComponent();

            RefreshRepositories();
        }

        private void RepositoriesMenuItem_Click(object sender, EventArgs e)
        {
            //_core.EditRepositoriesList();
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
