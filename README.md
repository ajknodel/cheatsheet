# cheatsheet
A searchable sqlite database using a bash wrapper for keeping data you want to be able to find easily in the future. A replacement for keeping huge text files (CTRL+F) or spreadsheets with random bits of data in them.

This is mostly published because it is the most useful bash script that I have written for my daily life.

## Usage:
First, make `cs` executable:

```
~ # chmod u+x cs
```

Second, open `cs` in a text editor and change the location of the 'cs.sqlite' database. I use Nextcloud to synchronize documents between computers I use and this works well to make sure I always have an updated copy of my notes.

The variable for this is at the top of 'cs' defined by `DBFILE`. You can also change `EDITOR` to be 'vim' if you do not like nano, for instance.

You can also add `cs` to your $PATH so that you don't have to invoke it with the full path.

Finally, run `cs` to see simple usage. The following is the output when you run `cs` without an argument:

```
Cheat Sheet: Used to keep notes in an organized fashion and find things easily without getting distracted

new: create a new entry
s: full search - display all entire entries that match search criteria; can take up to 5 key words to narrow down results
t: list only titles from results; can take up to 5 key words to narrow down results
show: display only the selected id #
update: Update an entry by id #, second argument is required
delete: Delete an entry by id #, second argument is required
```

When you create an entry using `cs new` it will open your chosen editor (by default nano) and allow you to add whatever notes you want to a database entry. When you exit the editor (after saving a temporary file), it will ask you if you want to add the entry to the database.

The database has two columns and one of them is an auto-numbering field and the other field is where you store your entry content. You use the auto-numbering field to select entries. The first line of the entry is the title of the entry. When you search using `cs t <search terms here>` you will be searching based on all provided arguments, but only the first line will appear as a search result. This allows you to filter through and find the entry you actually want to see.

When using `cs s` to search instead, it will display the full entries that match all search terms. All searches are performed using `AND` methodology. In other words, adding a keyword that does not exist in an entry will filter it out.