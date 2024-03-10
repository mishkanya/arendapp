#User
#{
#    "ok": true,
#    "result": {
#        "short_name": "Arendapp",
#        "author_name": "Arendapp",
#        "author_url": "",
#        "access_token": "98815802fe8b87c09b320647fee62577449b44e35cb6b3474599c5854f86",
#        "auth_url": "https://edit.telegra.ph/auth/wevf7tmS8IHQADIbdrLPRac3P5X2MhQitjyAMSqSA7"
#    }
#}


#Page
#{
#    "ok": true,
#    "result": {
#        "path": "ServerUrl-03-07",
#        "url": "https://telegra.ph/ServerUrl-03-07",
#        "title": "ServerUrl",
#        "description": "",
#        "views": 0,
#        "can_edit": true
#    }
#}


# https://api.telegra.ph/editPage/ServerUrl-03-07?access_token=98815802fe8b87c09b320647fee62577449b44e35cb6b3474599c5854f86&title=ServerUrl&content=[{"tag":"p","children":["https://be37-80-80-194-146.ngrok-free.app /"]}]&return_content=true



$pinfo = New-Object System.Diagnostics.ProcessStartInfo
$pinfo.FileName = "ngrok"
$pinfo.RedirectStandardError = $true
$pinfo.RedirectStandardOutput = $true
$pinfo.UseShellExecute = $false
$pinfo.Arguments = "http http://localhost:5151/"
$pinfo.CreateNoWindow = $false
$pinfo.RedirectStandardOutput = $true

$p = New-Object System.Diagnostics.Process
$p.StartInfo = $pinfo


Register-ObjectEvent -InputObject $p -EventName OutputDataReceived -Action {
    param([System.Object] $sender, [System.Diagnostics.DataReceivedEventArgs] $eventArgs)
    $eventArgs.Data
    "asdasd"
    
}
Start-Sleep -Seconds 2
$p.Start() 


<#

#Start-Process -FilePath ngrok -ArgumentList "http http://localhost:5151/"  -WindowStyle Maximized 

#Start-Sleep -Seconds 2

<#$process = "ngrok.exe"
Get-WmiObject Win32_Process -Filter "name = '$process'" #>


#>