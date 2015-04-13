**Active development of this project has moved to [github](https://github.com/pinky39/grove).**

Magicgrove is a computer implementation of a trading card game: Magic: The
Gathering. The goal of the project is to implement ai algorithms which can play
the game at the same level as a casual human player. Currently there are 690
unique cards implemented, mostly from Urza's block (593 out of 616 cards are
available). When compared to
[similar projects](http://www.slightlymagic.net/wiki/List_of_MTG_Engines) the
game excels at:

  * great user interface,
  * good 60-step lookahead ai with intelligent pruning algorithms,
  * sealed tournaments (up to 500 computer players),
  * draft tournaments,
  * save & load support,
  * easy & quick installation,
  * active development,
  * robustness.

## How AI is implemented ##

Magic: The Gathering is a game with hidden state. On every move, computer
simulates future moves and builds the game tree. Hidden information is not
available during simulation (computer is not cheating). Every leaf node is
scored and the best move is chosen according to min/max rule. Because the
unpruned game tree would take too long to evaluate, heuristics are used to
choose if the branch is expanded or not. Ai uses special rules for:

  * target selection,
  * timing of spells and abilities,
  * X cost calculation,
  * combat simulation,
  * mana payments,
  * repeated activations of abilities,
  * special card decisions

The game currently uses 60 steps look ahead, the search time rarely exceeds 5
seconds (on quad core machine).

### Deck generation ###
Limited decks are automatically generated from given starter and booster packs.
Computer first creates multiple decks for various color combinations, and then
chooses the one which wins a simulated tournament.

### Draft ###
Drafting AI is based on drafting strategies described here:
http://archive.wizards.com/Magic/magazine/article.aspx?x=mtgcom/academy/39.

## Contributing to the project ##
The easiest way to get familiar with source is
to try to add some new cards. Check out the 'Adding cards tutorial' series:

  1. [Adding cards Episode 1](AddingCardsEpisode1.md)
  1. [Adding cards Episode 2](AddingCardsEpisode2.md)

## Screen shots ##
![http://magicgrove.googlecode.com/git/doc/screnshots.jpg](http://magicgrove.googlecode.com/git/doc/screnshots.jpg)

## Release 3.0 (2014/08/11) ##

### New cards (140) ###

Academy Rector, Æther Sting, Ancient Silverback, Apprentice Necromancer, Archery
Training, Attrition, Aura Thief, Blizzard Elemental, Bloodshot Cyclops, Body
Snatcher, Braidwood Cup, Braidwood Sextant, Brass Secretary, Brine Seer,
Bubbling Beebles, Bubbling Muck, Caltrops, Capashen Knight, Capashen Standard,
Capashen Templar, Carnival of Souls, Chime of Night, Cinder Seer, Colos
Yearling, Compost, Covetous Dragon, Disappear, Disease Carriers, Donate Dying
Wail, Elvish Lookout, Elvish Piper, Emperor Crocodile, Encroach, Eradicate,
Extruder, False Prophet, Fatigue, Fend Off, Festering Wound, Field Surgeon,
Flame Jet, Fledgling Osprey, Flicker, Fodder Cannon, Gamekeeper, Goblin
Berserker, Goblin Festival, Goblin Gardener, Goblin Marshal, Goblin Masons,
Goliath Beetle, Heart Warden, Hulking Ogre, Hunting Moa, Illuminated Wings,
Impatience, Incendiary, Iridescent Drake, Ivy Seer, Jasmine Seer, Junk Diver,
Keldon Champion, Keldon Vandals, Kingfisher, Landslide, Lurking Jackals,
Magnify, Mantis Engine, Mark of Fury, Marker Beetles, Mask of Law and Grace,
Master Healer, Masticore, Mental Discipline, Metalworker, Metathran Elite,
Metathran Soldier, Momentum, Multani's Decree, Nightshade Seer, Opalescence o
Opposition, Pattern of Rebirth, Phyrexian Monitor, Phyrexian Negator o Plague
Dogs, Plated Spider, Plow Under, Powder Keg, Private Research, Quash, Rapid
Decay, Ravenous Rats, Rayne, Academy Chancellor, Reckless Abandon, Reliquary
Monk, Repercussion, Replenish, Rescue, Rofellos, Llanowar Emissary, Rofellos's
Gift, Sanctimony, Scent of Brine, Scent of Cinder, Scent of Ivy, Scent of
Jasmine, Scent of Nightshade, Scour, Serra Advocate, Sigil of Sleep, Skittering
Horror, Slinking Skirge, Solidarity, Soul Feast , Sowing Salt, Splinter,
Squirming Mass, Taunting Elf, Telepathic Spies, Temporal Adept, Tethered
Griffin, Thieving Magpie, Thorn Elemental, Thran Dynamo, Thran Foundry, Thran
Golem, Tormented Angel, Treachery, Trumpet Blast, Twisted Experiment, Urza's
Incubator, Voice of Duty, Voice of Reason, Wake of Destruction, Wild Colos,
Yavimaya Elder, Yavimaya Enchantress, Yavimaya Hollow, Yawgmoth's Bargain

### New sets & decks ###

  * New set: Urza's Destiny.
  * 19 preconstructed decks.
  * More than 13K pregenerated tournament decks.

### Improved UI ###

  * Animation when permanent leaves battlefield.
  * Animations when player looses or gains life.
  * Visual indication when player turn starts.
  * Visual indication when opponent plays a spell or activates an ability.
  * Better combat coloring for matching attackers and blockers.
  * Avatar selection in tournaments.
  * Changed shortcut from Spacebar to Enter when confirming multiple selections e.g when choosing attackers or blockers.

### Explicit ordering of triggered abilities ###

Players can now specify the order in which triggered abilities are
pushed to stack.

### Better AI ###

  * Improved combat AI.
  * Improved scoring AI.
  * Increased look ahead to 60 steps.

### Better resource management ###

  * Zipped resources.
  * Faster game load & progress indication.