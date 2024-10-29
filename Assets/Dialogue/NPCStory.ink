VAR speaker = ""
VAR alibi = ""
VAR clue1 = ""
VAR clue2 = ""
VAR clue3 = ""
VAR spokenTo = 0

-> DECISION
~ spokenTo = spokenTo + 1

== DECISION == 
{ spokenTo:
- 0: ->ALIBI
- 1: ->CLUE1
- 2: ->CLUE2
- 3: ->CLUE3
- else: ->EXTRA
}

== ALIBI ==
Excuse me? #speaker:Fluffy #portrait:Fluffy_Normal #layout:Left #audio:Fluffy
Yes? #speaker:{speaker} #portrait:{speaker}  #audio:{speaker}
Detective Fluffy, don't mind if I ask you a few questions? #speaker:Fluffy #portrait:Fluffy_Normal #layout:Left #audio:Fluffy
Go ahead #speaker:{speaker} #portrait:{speaker}  #audio:{speaker}
Where were you on the night of the 22nd? #speaker:Fluffy #portrait:Fluffy_Normal #layout:Left #audio:Fluffy
{alibi} #speaker:{speaker} #portrait:{speaker}  #audio:{speaker}
-> DONE

== CLUE1 ==
Do you have anything else you could tell me? #speaker:Fluffy #portrait:Fluffy_Normal #layout:Left #audio:Fluffy
{clue1}#speaker:{speaker} #portrait:{speaker}  #audio:{speaker}
-> DONE

== CLUE2 ==
Do you have anything else you could tell me? #speaker:Fluffy #portrait:Fluffy_Normal #layout:Left
{clue2} #speaker:{speaker} #portrait:{speaker}  #audio:Fluffy
-> DONE

== CLUE3 ==
Do you have anything else you could tell me? #speaker:Fluffy #portrait:Fluffy_Normal #layout:Left #audio:Fluffy
{clue3} #speaker:{speaker} #portrait:{speaker}  #audio:{speaker}
-> DONE

== EXTRA ==
Do you have anything else you could tell me? #speaker:Fluffy #portrait:Fluffy_Normal #layout:Left  #audio:Fluffy
Sorry, I've told you everything I know... #speaker:{speaker} #portrait:{speaker}  #audio:{speaker}
-> DONE