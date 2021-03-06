' AlphaBlendToolbar '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'
' Description: This addin creates a new toolbar in visual studio called
' "Alpha Toolbar" and then adds two new commands to it which feature
' alpha-transparent icon images.  The images are added directly via a
' reference to a System.Drawing.Bitmap, without needing to provide an
' index or resource ID.  Additionally, it is no longer necessary to provide
' a transparency mask to support transparency.
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

Imports System
Imports Microsoft.VisualStudio.CommandBars
Imports Extensibility
Imports EnvDTE
Imports EnvDTE80

Public Class Connect
	
    Implements IDTExtensibility2, IDTCommandTarget
	

    Private _applicationObject As DTE2
    Private _addInInstance As AddIn

    '''<summary>Implements the constructor for the Add-in object. Place your initialization code within this method.</summary>
    Public Sub New()

    End Sub

    '''<summary>Implements the OnConnection method of the IDTExtensibility2 interface. Receives notification that the Add-in is being loaded.</summary>
    '''<param name='application'>Root object of the host application.</param>
    '''<param name='connectMode'>Describes how the Add-in is being loaded.</param>
    '''<param name='addInInst'>Object representing this Add-in.</param>
    '''<remarks></remarks>
    Public Sub OnConnection(ByVal application As Object, ByVal connectMode As ext_ConnectMode, ByVal addInInst As Object, ByRef custom As Array) Implements IDTExtensibility2.OnConnection
        _applicationObject = CType(application, DTE2)
        _addInInstance = CType(addInInst, AddIn)

        If connectMode = ext_ConnectMode.ext_cm_Startup Or connectMode = ext_ConnectMode.ext_cm_AfterStartup Then
            Dim guids() As Object = {}
            Dim cmds As Commands2 = CType(_applicationObject.Commands, Commands2)

            ' Add new toolbar and set enabled and visible
            Dim cmdBars As CommandBars = CType(_applicationObject.CommandBars, CommandBars)
            Dim toolbar As CommandBar = cmdBars.Add("AlphaToolbar", MsoBarPosition.msoBarTop, System.Type.Missing, True)
            toolbar.Visible = True
            toolbar.Enabled = True

            ' Make sure the alpha command doesn't already exist
            Dim commandAlpha As Command = Nothing
            Dim commandOmega As Command = Nothing

            Try
                commandAlpha = cmds.Item("AlphaBlendToolbar.Connect.AlphaButton")
            Catch ex As System.ArgumentException
                ' Load the alpha button image from a 32-bit alpha-transparent portable network graphic file which is an 
                ' embedded resource in this project.  This image could also be loaded directly from disk.
                Dim alpha As System.Drawing.Bitmap = New System.Drawing.Bitmap(GetType(AlphaBlendToolbar.Connect), "alpha.png")

                ' Create the alpha button command, specifying the command name, button text, tooltip, bitmap image,
                ' status, style, and control type.  Add this command to the AlphaToolbar.
                commandAlpha = cmds.AddNamedCommand2(_addInInstance, "AlphaButton", "Alpha", "Alpha Command", False, alpha, guids,
                    vsCommandStatus.vsCommandStatusEnabled, vsCommandStyle.vsCommandStylePictAndText, vsCommandControlType.vsCommandControlTypeButton)
            End Try
            commandAlpha.AddControl(toolbar, 1)

            ' Make sure the omega command doesn't already exist
            Try
                commandOmega = cmds.Item("AlphaBlendToolbar.Connect.OmegaButton")
            Catch ex As System.ArgumentException
                ' Load the omega button image from a 32-bit alpha-transparent portable network graphic file which is an 
                ' embedded resource in this project.  This image could also be loaded directly from disk.
                Dim omega As System.Drawing.Bitmap = New System.Drawing.Bitmap(GetType(AlphaBlendToolbar.Connect), "omega.png")

                ' Create the omega button command, specifying the command name, button text, tooltip, bitmap image,
                ' status, style, and control type.  Add this command to the OmegaToolbar.
                commandOmega = cmds.AddNamedCommand2(_addInInstance, "OmegaButton", "Omega", "Omega Command", False, omega, guids,
                    vsCommandStatus.vsCommandStatusEnabled, vsCommandStyle.vsCommandStylePictAndText, vsCommandControlType.vsCommandControlTypeButton)
            End Try
            commandOmega.AddControl(toolbar, 2)
        End If
    End Sub

    '''<summary>Implements the OnDisconnection method of the IDTExtensibility2 interface. Receives notification that the Add-in is being unloaded.</summary>
    '''<param name='disconnectMode'>Describes how the Add-in is being unloaded.</param>
    '''<param name='custom'>Array of parameters that are host application specific.</param>
    '''<remarks></remarks>
    Public Sub OnDisconnection(ByVal disconnectMode As ext_DisconnectMode, ByRef custom As Array) Implements IDTExtensibility2.OnDisconnection
    End Sub

    '''<summary>Implements the OnAddInsUpdate method of the IDTExtensibility2 interface. Receives notification that the collection of Add-ins has changed.</summary>
    '''<param name='custom'>Array of parameters that are host application specific.</param>
    '''<remarks></remarks>
    Public Sub OnAddInsUpdate(ByRef custom As Array) Implements IDTExtensibility2.OnAddInsUpdate
    End Sub

    '''<summary>Implements the OnStartupComplete method of the IDTExtensibility2 interface. Receives notification that the host application has completed loading.</summary>
    '''<param name='custom'>Array of parameters that are host application specific.</param>
    '''<remarks></remarks>
    Public Sub OnStartupComplete(ByRef custom As Array) Implements IDTExtensibility2.OnStartupComplete
    End Sub

    '''<summary>Implements the OnBeginShutdown method of the IDTExtensibility2 interface. Receives notification that the host application is being unloaded.</summary>
    '''<param name='custom'>Array of parameters that are host application specific.</param>
    '''<remarks></remarks>
    Public Sub OnBeginShutdown(ByRef custom As Array) Implements IDTExtensibility2.OnBeginShutdown
    End Sub

    ''' <summary>Implements the QueryStatus method of the IDTCommandTarget interface. This is called when the command's availability is updated</summary>
    ''' <param name='CmdName'>The name of the command to determine state for.</param>
    ''' <param name='NeededText'>Text that is needed for the command.</param>
    ''' <param name='StatusOption'>The state of the command in the user interface.</param>
    ''' <param name='CommandText'>Text requested by the neededText parameter.</param>
    ''' <seealso class='Exec' />
    Public Sub QueryStatus(ByVal CmdName As String, ByVal NeededText As vsCommandStatusTextWanted, ByRef StatusOption As vsCommandStatus, ByRef CommandText As Object) Implements IDTCommandTarget.QueryStatus
        If NeededText = vsCommandStatusTextWanted.vsCommandStatusTextWantedNone Then
            ' Let the caller know that the alpha command is supported and enabled
            If CmdName = "AlphaBlendToolbar.Connect.AlphaButton" Then
                StatusOption = vsCommandStatus.vsCommandStatusSupported Or vsCommandStatus.vsCommandStatusEnabled
                Return
            ElseIf CmdName = "AlphaBlendToolbar.Connect.OmegaButton" Then
                ' Let the caller know that the omega button is supported and enabled
                StatusOption = vsCommandStatus.vsCommandStatusSupported Or vsCommandStatus.vsCommandStatusEnabled
                Return
            End If
        End If
    End Sub

    '''<summary>Implements the Exec method of the IDTCommandTarget interface. This is called when the command is invoked.</summary>
    '''<param name='CmdName'>The name of the command to execute.</param>
    '''<param name='ExecuteOption'>Describes how the command should be run.</param>
    '''<param name='VariantIn'>Parameters passed from the caller to the command handler.</param>
    '''<param name='VariantOut'>Parameters passed from the command handler to the caller.</param>
    '''<param name='Handled'>Informs the caller if the command was handled or not.</param>
    '''<seealso class='Exec' />
    Public Sub Exec(ByVal CmdName As String, ByVal ExecuteOption As vsCommandExecOption, ByRef VariantIn As Object, ByRef VariantOut As Object, ByRef Handled As Boolean) Implements IDTCommandTarget.Exec
        Handled = False

        If ExecuteOption = vsCommandExecOption.vsCommandExecOptionDoDefault Then
            ' Handle the alpha button command
            If CmdName = "AlphaBlendToolbar.Connect.AlphaButton" Then
                System.Windows.Forms.MessageBox.Show("Alpha")
                Handled = True
                Return
            ElseIf CmdName = "AlphaBlendToolbar.Connect.OmegaButton" Then
                System.Windows.Forms.MessageBox.Show("Omega")
                Handled = True
                Return
            End If
        End If
    End Sub
End Class
