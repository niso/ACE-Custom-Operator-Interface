# ACE Custom Operator Interface

Took AceDemo.zip from ACE4.5 distribution and used Custom Operator Interface as seed for this project.

Scope of the project is to manipulate a set of V+ Doubles to parameterize the motions of a FlexiBowl.
The user can set and test parameters of different motions by pressing the Execute button behind each set.
Before any data can be manipulated, the controller must be connected and a recipe must be selected.
The UI enforces selecting a recipe - if not done before connecting to the controller, a recipe "New FlexiBowl Recipe" automatically gets created/set.
Each modified value automatically will be saved to the recipe (currently there is no undo).
