#!/bin/bash
# Cheat sheet - cs for short

DBFILE="$HOME/Nextcloud/cs.sqlite"
EDITOR="nano -c"

function usage() {
	printf "Cheat Sheet: Used to keep notes in an organized fashion and find things easily without getting distracted\n\n"
	echo 'new': create a new entry
	echo 's': full search - display all entire entries that match search criteria\; can take up to 5 key words to narrow down results
	echo 't': list only titles from results\; can take up to 5 key words to narrow down results
	echo 'show': display only the selected id \#
	echo 'update': Update an entry by id \#, second argument is required
	printf "delete: Delete an entry by id #, second argument is required\n"
}

# new
if [ "$#" -eq 1 ] && [ $1 == 'new' ]; then
	tmpfile="/tmp/$(strings /dev/urandom | grep -o '[[:alnum:]]' | head -n 12 | tr -d '\n'; echo)-new-entry.txt"
	echo New entry!
	$EDITOR `echo $tmpfile`

	read -r -p "Are you sure you want to update this entry? [y/n] " response
	if [ $response == "y" ]; then
		sqlite3 $DBFILE "insert into main (commands) values ('$(cat $tmpfile | sed "s/'/\\'\\'/g")')"\
		&& echo Entry added && cat $tmpfile
		rm $tmpfile
		sleep 1
		echo New Entry ID: $(sqlite3 $DBFILE "select max(entryid) from main order by entryid desc limit 1")
	else
		echo Entry not added
	fi
fi

# search
if [ "$#" -ge 2 ] && [ $1 == 's' ] && [ $2 != 'all' ]; then
	if [ "$#" -eq 2 ]; then
		sqlite3 $DBFILE "select * from main where commands LIKE '%$2%'" \
		| sed -e '/[0-9]|/i \
		==========================================================' \
		| sed -e '/[0-9]|/a \
		=========================================================='
	elif [ "$#" -eq 3 ]; then
		sqlite3 $DBFILE "select * from main where commands LIKE '%$2%' AND commands LIKE '%$3%'" \
		| sed -e '/[0-9]|/i \
		==========================================================' \
		| sed -e '/[0-9]|/a \
		=========================================================='
	elif [ "$#" -eq 4 ]; then
		sqlite3 $DBFILE "select * from main where commands LIKE '%$2%' AND commands LIKE '%$3%' AND commands LIKE '%$4%'" \
		| sed -e '/[0-9]|/i \
		==========================================================' \
		| sed -e '/[0-9]|/a \
		=========================================================='
	elif [ "$#" -eq 5 ]; then
		sqlite3 $DBFILE "select * from main where commands LIKE '%$2%' AND commands LIKE '%$3%' AND commands LIKE '%$4%' AND commands LIKE '%$5%'" \
		| sed -e '/[0-9]|/i \
		==========================================================' \
		| sed -e '/[0-9]|/a \
		=========================================================='
	elif [ "$#" -eq 6 ]; then
		sqlite3 $DBFILE "select * from main where commands LIKE '%$2%' AND commands LIKE '%$3%' AND commands LIKE '%$4%' AND commands LIKE '%$5%' AND commands LIKE '%$6%'" \
		| sed -e '/[0-9]|/i \
		==========================================================' \
		| sed -e '/[0-9]|/a \
		=========================================================='
	fi
elif [ "$#" -eq 1 ] && [ $1 == 's' ]; then
	echo search requires an argument or the the word \'all\' to list all contents
elif [ "$#" -eq 2 ] && [ $1 == 's' ] && [ $2 == 'all' ]; then
	sqlite3 $DBFILE "select * from main" | sed -e '/[0-9]|/i \
	==========================================================' \
	| sed -e '/[0-9]|/a \
	=========================================================='
fi

# show
if [ "$#" -ge 2 ] && [ $1 == 'show' ] && [ $2 != 'all' ]; then
	sqlite3 $DBFILE "select * from main where entryid = '$2'" \
	| sed -e '/[0-9]|/i \
	==========================================================' \
	| sed -e '/[0-9]|/a \
	=========================================================='
elif [ "$#" -eq 1 ] && [ $1 == 'search' ]; then
	echo show requires an id \# to view an entry
fi

# title
if [ "$#" -ge 2 ] && [ $1 != 'update' ] && [ $1 == 't' ] && [ $2 != 'all' ]; then
	echo '=========================================================='
	if [ "$#" -eq 2 ]; then
		sqlite3 $DBFILE "select * from main where commands LIKE '%$2%'" \
		| sed -e '/[0-9]|/a \
		==========================================================' \
		| egrep '=============|[0-9]\|'
	elif [ "$#" -eq 3 ]; then
		sqlite3 $DBFILE "select * from main where commands LIKE '%$2%' AND commands LIKE '%$3%'" \
		| sed -e '/[0-9]|/a \
		==========================================================' \
		| egrep '=============|[0-9]\|'
	elif [ "$#" -eq 4 ]; then
		sqlite3 $DBFILE "select * from main where commands LIKE '%$2%' AND commands LIKE '%$3%' AND commands LIKE '%$4%'" \
		| sed -e '/[0-9]|/a \
		==========================================================' \
		| egrep '=============|[0-9]\|'
	elif [ "$#" -eq 5 ]; then
		sqlite3 $DBFILE "select * from main where commands LIKE '%$2%' AND commands LIKE '%$3%' AND commands LIKE '%$4%' AND commands LIKE '%$5%'" \
		| sed -e '/[0-9]|/a \
		==========================================================' \
		| egrep '=============|[0-9]\|'
	elif [ "$#" -eq 6 ]; then
		sqlite3 $DBFILE "select * from main where commands LIKE '%$2%' AND commands LIKE '%$3%' AND commands LIKE '%$4%' AND commands LIKE '%$5%' AND commands LIKE '%$6%'" \
		| sed -e '/[0-9]|/a \
		==========================================================' \
		| egrep '=============|[0-9]\|'
	fi
elif [ "$#" -eq 1 ] && [ $1 == 't' ]; then
	echo title requires an argument or the the word \'all\' to list all contents
elif [ "$#" -eq 2 ] && [ $1 == 't' ] && [ $2 == 'all' ]; then
	echo '=========================================================='
	sqlite3 $DBFILE "select * from main" | sed -e '/[0-9]|/a \
	==========================================================' \
	| egrep '=============|[0-9]\|'
fi

# update
if [ "$#" -eq 2 ] && [ $1 == 'update' ]; then
	tmpfile="/tmp/$2-update-entry.txt"
	sqlite3 $DBFILE "select commands from main where entryid = $2" > $tmpfile
	$EDITOR $tmpfile

	read -r -p "Are you sure you want to update this entry? [y/n] " response
	if [ $response == "y" ]; then
		sqlite3 $DBFILE "UPDATE main set commands = '$(cat $tmpfile | sed "s/'/\\'\\'/g")' WHERE entryid == $2"\
		 && echo Updated entry as follows: && echo Entry id: $2
		cat $tmpfile
	else
		echo Database not updated
	fi

	rm $tmpfile
fi

#delete
if [ "$#" -eq 2 ] && [ "$1" == 'delete' ]; then
        echo "--->  $(sqlite3 $DBFILE "select * from main where entryid = '$2'" | egrep '=============|[0-9]\|')"

	read -r -p "Are you sure you want to delete this entry? [y/n] " response
	if [ $response == "y" ]; then
		sqlite3 $DBFILE "DELETE from main where entryid = '$2'" && echo Deleted record $2.
	else
		echo Database entry not deleted.
	fi
elif [ "$1" == 'delete' ] && [ -z "$2" ]; then
	echo Deleting an entry requires a entry id \#
fi

# help
if [ "$#" -eq 0 ]; then
	usage
fi

