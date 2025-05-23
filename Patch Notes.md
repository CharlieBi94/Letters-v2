Patch Notes
May 23 2025
- Changed levelup options into powerpacks and triggered by placeholder button
- Updated rows to have a maximum tile limit (this eliminates 6519 words from being formed)

May 22 2025
- Updated some variables in the difficulty settings into curves for easier scaling

May 19 2025
- Moved the god mode effect behind the timer and score text
- Fixed offset of floating text so it appears more close to the center of the row
- Fixed issue with timer data not updating during god mode
- Changed the cat the animation to occcur for system added tiles

May 13 2025
- Hooked up the fading text to timer changes when submitting answers
- Added curve for spawning double letters as correct answers increase

May 12 2025
- Updated instructions misalignment on page 4 of instructions screen
- Updated code to allow player to move system added tiles during god mode
- Updated row behaviour to remove all empty rows at the end of god mode
- Fixed small bug where system was always spawning consonants in single letters (now random again)
- Created Scrolling + Fade floating text animation component

May 5 2025
- Added mute button to pause menu
- Built simple framework for carousel UI
- Added tutorial to instructions menu

April 29 2025
- Added moving background 
- Started reworking some UI elements

April 24 2025
- Added tooltips to difficulty settings in inspector
- Updated row spawning to occur on a timer
- now generates a guarenteed vowel when spawning a double letter  row
- Added progress bar to indicated incoming new row spawn
- Added diminishing returns for submitting the same word
- Added additional variables for calculating score and time
- Added variables for god mode count down

April 23 2025
- Added cat animation for spawning letters
- Fixed bug to allow player to spawn row with click in god mode
- Fixed bug where ending god mode restarted the game if the game happened to be game over
- Added config to allow different 
- Added coutdown to god mode
- Added ability for system to spawn multiple (2) letters in a row
- Added configuration option to game manager for above system
