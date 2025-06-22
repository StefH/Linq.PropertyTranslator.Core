rem https://github.com/StefH/GitHubReleaseNotes

SET version=1.0.5.1

GitHubReleaseNotes --output ReleaseNotes.md --skip-empty-releases --exclude-labels question invalid documentation --version %version% --token %GH_TOKEN%