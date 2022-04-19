### Simplification
Because of limitation of implementation time there is some known issues:
- The app doesn't contain the separated DAL layer. For the production purposses need to implement separate project PokemonDAL.proj, which contains DB entities and migrations. 
- It's better to separate logging/caching/mapping/common logic to it's own assembly to have ability to share this logic with different layers of application.
- For more complex business logic and for the availability purpose it's better to implement the separate service with following flow: API - PokemonService - DAL. It allows to have ability to multiply services to support the high load. Services might be connected via gRPC protocol.

### Objective

Your assignment is to create a Pokémon API from a CSV file using C# and .Net.

### Brief

Professor Oak is in trouble! A wild Blastoise wreaked havoc in the server room and destroyed every single machine. There are no backups - everything is lost! Professor Oak quickly scribbles down all the Pokémon from memory and hands them to you on a piece of paper. (`/Data/pokemon.csv`). Your task is to restore the Pokémon Database from that file and create a Pokémon API so that they’re never lost again.

### Tasks

-   Implement assignment using:
    -   Language: **C#**
    -   Framework: **.Net**
-   Create a Pokémon Model that includes all fields outlined in `/Data/pokemon.csv`
-   Parse the .csv file and create entries for each row based on the following conditions:
    -   Exclude Legendary Pokémon
    -   Exclude Pokémon of Type: Ghost
    -   For Pokémon of Type: Steel, double their HP
    -   For Pokémon of Type: Fire, lower their Attack by 10%
    -   For Pokémon of Type: Bug & Flying, increase their Attack Speed by 10%
    -   For Pokémon that start with the letter **G**, add +5 Defense for every letter in their name (excluding **G**)
-   Create one API endpoint `/pokemon`
    -   This API endpoint should be searchable, filterable and paginatable
        -   Search: name
            -   Bonus: implement fuzzy search using Levenshtein distance
        -   Filter: HP, Attack & Defense
            -   e.g. `/pokemon?hp[gte]=100&defense[lte]=200`
        -   Pagination: e.g. `/pokemon?page=1`

### Evaluation Criteria

-   **C#** best practices
-   Show us your work through your commit history
-   We're looking for you to produce working code, with enough room to demonstrate how to structure components in a small program
-   Completeness: did you complete the features?
-   Correctness: does the functionality act in sensible, thought-out ways?
-   Maintainability: is it written in a clean, maintainable way?
-   Testing: is the system adequately tested?

### CodeSubmit

Please organize, design, test and document your code as if it were going into production - then push your changes to the master branch. After you have pushed your code, you may submit the assignment on the assignment page.

All the best and happy coding,

The Codified Team
