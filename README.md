# Chicken Siege

Chicken Siege is a top down tower defense game made in unity, scripted with C#. The goal is to stop the chickens along the path to the castle using towers. If too many chickens reach the castle, you lose. If you clear all of the waves of chickens, you win! Manage your money wisely and place towers strategically to beat all six levels. If you would like to see images from the game, click [here](/ImageGallery.pdf).

### Table of Contents

- [Prerequisites](#prerequisites)
- [Installing](#installing)
- [How to Play](#how-to-play)
- [Testing](#testing)
- [Built With](#built-with)
- [Author](#author)
- [License](#license)
- [Acknowledgements](#acknowledgments)

### Prerequisites

To install and use the project file, a version of the [unity editor](https://unity.com/) is required. To simply play the game, there are no prerequisites.

### Installing

To install and use project file, download the github repository, unzip the zipped folder and import the "project" folder into your Unity. Note: The game was built on Unity version 2022.3.14f1 and is untested on other 
versions.

To install the game you can either download the github repository and find the build in the "finalbuild" folder, or if you wish to download only the game itself you can visit [this](https://drive.google.com/drive/folders/1qUK_o2G0qrgref8voAjVZdAlPwlm85w1?usp=sharing) google drive link and download the zipped folder.

## How to Play

Chicken Siege is controlled entirely with the mouse. To select a level click "Level Select" from the main menu and click a level of your choice. Once in the level, your total coins and health can be seen in the top left corner. If a chicken reaches the end of the path, seen in the middle of the screen, you lose health. Health cannot be regained. You spend coins on towers and tower upgrades. You gain coins from defeating chickens. The bottom left hand corner of the screen hosts the towers, click on any of these then click on an open tile to place a tower. Clicking on a placed tower will bring up an upgrade menu allowing you to upgrade the tower provided you have enough coins. Once you've placed towers you can start the wave by pressing the button labled "Next Wave" in the top right hand corner. Chickens should spawn and your towers should attack the chickens. Once you clear all of the chickens in a wave you will be able to start another wave. Once you clear all of the waves, the game is over. To see a more detailed tutorial, select the "Tutorial" button from the main menu of the game.

## Testing

The testing of Chicken Siege consists of two testing method. The first tests are technical unit tests written in c# and conducted in Unitys own test runner. The second test method is a practical playtest of the game conducted by volunteers who were willing to fill out a survery to share their experience with the game. Visit [this](/test/unit_tests) folder to view the results of the Unit Tests. Visit [this](/test/player_tests/ChickenSiegePlaytestSurvey.pdf) folder to view the results of the player tests.

## Built With

* [Unity](https://unity.com/) - The engine used
* [Visual Studio](https://visualstudio.microsoft.com/) - The IDE used

## Author

* **Brayden Ward**, Senior at Northeastern State University
  
## License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details

## Acknowledgments

* Dr. Bekkering at Northeastern State University
* My good friends who were willing to test the game
