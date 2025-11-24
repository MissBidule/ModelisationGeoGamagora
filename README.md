# ModelisationGeoGamagora

To launch the exercises simply execute the Unity projects in the folders to open it. 

# TP1

To see the different shape made, simply go to the "shape" named GameObject in the hierarchy and activate ONLY one script of your choice depending on the shape you want to see.
Then adjust the parameters of the shapes to your liking and press play.
You should see the shapes take form in the editor.

# TP2

In the shapes folder you can add an OFF file which can be read on the shape GameObject simply by writing its name on the off reader script. If you want it to automatically rewrite it centered and auto sized on a file, check the auto saved tick box (know that the size of the file will increment the launch time consequently). The new file will be in the ShapesOutput directory.

# TP3

The object spatial enum has different options : 
    - Max depth is for the smaller cube that can be done (7 is good enough to not make unity crash)
    - Spheres determined the spheres inside the bounding box, their diameter and their center (for the draw mode we advise a sphere greater than the box)
    - Size is the width and height of the first bounding box
    - Position is the center of the first bounding box
    - Adaptative says whether or not we stop the slicing if the box is fully inside (for the draw mode turn it off)
    - State is for making union or intersection when multiple spheres are set
    - Visibility determines if the cubes inside the bounding box are visible or not by default

You can move the Brush gameObject to show or erase what is in contact with it. If the cubes are not visible all cubes in contact will be. And vice versa.