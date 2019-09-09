$region = "<regionGoesHere>"
$apiKey = "<apiKeyGoesHere>"


Invoke-WebRequest `
-Uri "https://$region.api.cognitive.microsoft.com/vision/v1.0/analyze?visualFeatures=Description" `
-Method POST `
-Headers @{
   "Ocp-Apim-Subscription-Key"="$apiKey"; 
   "Content-Type"="application/octet-stream"
} `
-InFile ".\image1.jpg" `
`
|%{$_.Content-replace"([\[{,])","`$1`n"-replace"([\]}])","`n`$1"}