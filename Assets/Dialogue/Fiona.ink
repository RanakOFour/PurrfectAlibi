VAR alibi = ""
VAR clue1 = ""
VAR clue2 = ""
VAR clue3 = ""
VAR spokenTo = 0

{ spokenTo:
- 0: ->ALIBI
- 1: ->CLUE1
- 2: ->CLUE2
- 3: ->CLUE3
- else: ->EXTRA
}

== ALIBI ==
Excuse me, ma'am? #speaker:Fluffy #portrait:Fluffy_Normal #layout:Left
Yes? #speaker:Fiona #portrait:Fiona #layout:Right
Detective Fluffy, don't mind if I ask you a few questions? #speaker:Fluffy #portrait:Fluffy_Normal #layout:Left
Go ahead #speaker:Fiona #portrait:Fiona #layout:Right
Where were you on the night of the 22nd? #speaker:Fluffy #portrait:Fluffy_Normal #layout:Left
{alibi} #speaker:Fiona #portrait:Fiona #layout:Right
~ spokenTo = spokenTo + 1
-> DONE

== CLUE1 ==
Do you have anything else you could tell me? #speaker:Fluffy #portrait:Fluffy_Normal #layout:Left
{clue1} #speaker:Fiona #portrait:Fiona #layout:Right
~ spokenTo = spokenTo + 1
-> DONE

== CLUE2 ==
Do you have anything else you could tell me? #speaker:Fluffy #portrait:Fluffy_Normal #layout:Left
{clue2} #speaker:Fiona #portrait:Fiona #layout:Right
~ spokenTo = spokenTo + 1
-> DONE

== CLUE3 ==
Do you have anything else you could tell me? #speaker:Fluffy #portrait:Fluffy_Normal #layout:Left
{clue3} #speaker:Fiona #portrait:Fiona #layout:Right
~ spokenTo = spokenTo + 1
-> DONE

== EXTRA ==
Do you have anything else you could tell me? #speaker:Fluffy #portrait:Fluffy_Normal #layout:Left
Sorry, I've told you everything I know... #speaker:Fiona #portrait:Fiona #layout:Right
-> DONE