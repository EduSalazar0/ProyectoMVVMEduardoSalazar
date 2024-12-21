namespace ProyectoMVVMEduardoSalazar.Views;

public partial class AllNotePage : ContentPage
{
	public AllNotePage()
	{
		InitializeComponent();
	}
    private void ContentPage_NavigatedTo(object sender, NavigatedToEventArgs e)
    {
        notesCollection.SelectedItem = null;
    }
}