# pom - a pomodoro command line interface

## Installation
1. Clone this repository
2. In the repository, run `$ dotnet publish --runtime linux-x64 --self-contained --configuration Release --output </your/output/directory>`
3. Add your output directory to your `$PATH`. In Ubuntu, this can be done in `/etc/environment`.

## Usage
- `$ pom start`
    - Start a 25 minute pomodoro, followed by a 5 minute break
- `$ pom left`
    - Get minutes left of pomodoro. Example: "🍅 19".
    - When the pomodoro is over, get minutes left of break. Example: "☕ 3".
    - When the break is over, get an empty string.
