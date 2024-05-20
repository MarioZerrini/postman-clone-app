using PostmanCloneLibrary;

namespace PostmanCloneUI;

public partial class Dashboard : Form
{

    private readonly IApiAccess api = new ApiAccess(); // to get ready for dependency injection
    public Dashboard()
    {
        InitializeComponent();
        httpVerbSelection.SelectedItem = "GET";
    }

    private async void callApi_Click(object sender, EventArgs e)
    {
        systemStatus.Text = "Calling API...";
        resultsText.Text = "";

        // Validate the API URL
        if (api.IsValidUrl(apiText.Text) == false)
        {
            systemStatus.Text = "Invalid URL...";
            return;
        }

        try
        {
            systemStatus.Text = "Calling API...";

            resultsText.Text = await api.CallApiAsync(apiText.Text); // await says: we are gonna let the ui have control until we are done with this call and we are gonna pause the code on this line
            callData.SelectedTab = resultsTab;

            systemStatus.Text = "Ready";
        }
        catch (Exception ex)
        {

            resultsText.Text = "Error: " + ex.Message;
            systemStatus.Text = "Error";
        }
    }
}
