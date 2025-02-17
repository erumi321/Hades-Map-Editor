Hades Map Editor is a windows application made with WinForms. It can import ".map_text" and will eventually export as well. It creates its own format ".hades_map" to manage project data while working on a map. 

To Start: make sure you go to Files -> Parameters -> Paths. And make sure to add all paths you have accessible. 
- Resources Path: A temporary folder that will contains all map assets coming from ".pkg".
- Default Project Path: A folder to manage new ".hades_map" project will be created and act as repository. (CURRENTLY UNIMPLEMENTED)
- Python Path: A folder hosting your "python.exe". By default it should be "C:\...\AppData\Local\Microsoft\WindowsApp". Also, Python needs to have the extension (deppth)[https://github.com/quaerus/deppth] to extract access directly from Hades Game Folder.
- Hades Path: Folder hosting the Hades game. E.T: "C:\Hades".
Then save by clicking the save button below.

With paths setup, you can go to Asset -> Fetch Raw Assets. It will run python sub-routine to extract assets from the game and add them to the resources folder. You'll see the progress in the status bar below.
Once done, you can go to Asset -> Compile Assets. It will compile a library of assets to be imported in the editor. You'll see the progress in the status bar below.
Once done, you'll see the assets in the asset manager on the right side.

You can then import a map with `Files -> Open .thing_text`, this will open two prompts to choose a .thing_bin and .thing_text file. Or use `Files -> Open .hades_map` to open a project file.

You can then export a map with `Files -> Export As` and choose a location to export a `.thing_bin` which the game will read.

Features in place:
- Click in the Element Manager to see what metadata it has. It will display the metadata in the metadata manager.
- Edit metadata
- See Map Manager in the middle, but only loaded assets are viewable.
- Export map to binary
- Toggle visible groups in editor