# Adding cards Episode 1 #

Hi, fellow developer, thank you for your interest in Magicgrove.
This series will try to explain how you can add new cards to the Magicgrove. You will need:

  * [Visual Studio 2010](http://www.microsoft.com/visualstudio/eng/downloads#d-2010-express)
  * [Git client for windows](http://code.google.com/p/msysgit/downloads/list?q=full+installer+official+git)

In the first part of this tutorial you will learn how to:

  * prepare your working environment to compile the project yourself,
  * implement a card named 'Akroma, Angel of Wrath',
  * write an automated test to check that the card works as expected,
  * share your work.

## Preparing your working environment ##

First install Visual Studio and Git using provided installers.
Open command prompt and type the following commands.

```
cd \
md Proj
cd Proj
git clone https://code.google.com/p/magicgrove/

```

Project source code should now be available in c:\Proj\magicgrove. Type

```
cd magicgrove
build.bat
```

This will compile the project, run all tests and create a new release in the Release subfolder.

## Implement card 'Akroma, Angel of Wrath' ##

Before you start coding, you need to create your private branch in git, this will allow you to pull updates to master without affecting your work. To do this, open console and go to magicgrove directory, then type:

```
git checkout -b myNewCards
```

This will create a new branch named 'myNewCards'.

Now, open the project by clicking on Grove.sln. The solution contains 3 projects:

  * Grove (main project used to produce grove.exe)
  * Grove.Tests (automated tests)
  * Grove.Utils (utils used to automate development tasks, e.g. generate sealed decks...)

In the Grove project create a new file inside the Cards folder named AkromaAngelOfWrath. Copy and paste the following code into the file:

```

namespace Grove.Cards
{
  using System.Collections.Generic;
  using Gameplay.Abilities;
  using Gameplay.Characteristics;
  using Gameplay.Misc;

  public class AkromaAngelOfWrath : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Akroma, Angel of Wrath")
        .ManaCost("{5}{W}{W}{W}")
        .Type("Legendary Creature Angel")
        .Text("Flying, first strike, vigilance, trample, haste, protection from black and from red")
        .FlavorText("No rest. No mercy. No matter what.")
        .Power(6)
        .Toughness(6)
        .SimpleAbilities(Static.Flying, Static.FirstStrike, Static.Vigilance, Static.Trample, Static.Haste)
        .Protections(CardColor.Black)
        .Protections(CardColor.Red);
    }
  }
}

```

Let's look at the code in detail.

  1. By convention, all card definitions must be in namespace Grove.Cards (the engine only loads cards in this namespace).
  1. We see that the code declares a class named AkromaAngelOfWrath that is derived from a class named CardTemplateSource. CardTemplateSource contains an abstract method GetCards() which all derived classes should implement. This method returns an enumerable of CardTemplate objects. A CardTemplate defines the properties of the card and is used by the engine to produce concrete instances of cards. For example, if a deck contains 4 copies of Akroma, Angel of Wrath, this object would be called 4 times to create 4 Card objects.
  1. A card definition usually starts by specifying card's name, mana cost, type, text and flavor text. This is done by calling corresponding methods on CardTemplate object. Since 'Akroma, Angel of Wrath' is a creature, we also specify its power and toughness.
  1. Simple static abilities like flying, first strike ... are defined by calling SimpleAbilities method.
  1. Protections from colors are defined by calling Protections method.

All clear? Let's compile and run the project to check our newly implemented card. Run the project by pressing F5, then open deck editor. Your card should appear in the library.

## Verifying implementation with automated tests ##

Let's verify that our card works as expected. How do we do that? We have a couple of options:

  * we could create a deck and play a game with it to see if it works,
  * we could write a small program which would play the card instead of us.

The advantage of the first option is that it is fairly straight-forward and it tests both the user experience and the ai. The disadvantage is that it takes a lot of time. The second option is faster, and if the card does not work as expected, we can easily debug the code and find the cause. It also allows us to check the behaviour of the card on every build, we just run the program again. The disadvantage is that it may cause some issues with the user interface, which we will not detect.

For this simple card, writing automated tests will be enough. In the folder Cards inside the Grove.Tests project create a file named AkromaAngelOfWrath, then copy the following code into it:

```
namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class AkromaAngelOfWrath
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void PlayAndAttack()
      {
        Hand(P1, "Akroma, Angel of Wrath");
        
        Battlefield(P1, "Plains", "Plains", "Plains", "Plains", "Plains", "Plains", "Plains", "Plains");
        Battlefield(P2, "Verdant Force");

        RunGame(1);

        Equal(14, P2.Life);
      }
    }
  }
}
```

Let's look at the code in detail.

  1. First we define a container class that will contain our tests named AkromaAngelOfWrath.
  1. Inside the container class, we define another class named Ai, derived from class AiScenario. This tells test engine that we wish both players to be controlled by Ai. Magicgrove supports 3 kinds of 'scenario' tests:
    * Ai tests - both players are controlled by Ai
    * Predefined tests - both players are controlled by the test script
    * Predefined Ai tests - the first player is controlled by the script, the second player is controlled by Ai.
  1. Our test method is called PlayAndAttack. We expect that player 1 will play Akroma from his hand and attack with it (Verdant Force would be unable to block since Akroma has flying). The game is run for 1 turn, we expect player 2 life to be 14 at the end of the turn.

To run the test manualy, run xunit.gui.clr4.x86.exe located in lib\xunit directory, and browse for grove.test.dll assembly located in \test\Grove.Tests\bin\Debug. Type 'akroma' in the search field, select the appropriate test and run it. The test should pass. Another option is to execute tests directly from Visual Studio using a runner such as [TestDriven.Net](http://www.testdriven.net/quickstart.aspx) or [Resharper](https://www.jetbrains.com/resharper/).

This test will also be to run when you build the application using build.bat.

## Sharing your work ##
Congratulations, you have successfully implemented your first card! If you wish that others will be able to play with your card, you need to submit it to the mailing list.
First you need to configure your name and email address with git. These will identify you as project contributer. To do this, open console, go to the magicgrove root directory and type:

```
git config user.name "Your name"
git config user.email "Your@email"
```

Now stage and commit your changes by typing:

```
git add -A
git commit
```

Enter the commit message, e.g 'Added card: Akroma, Angel of Wrath'. After committing your changes, generate a patch file by typing:


```
git format-patch -M origin/master -o c:
```

This will create a .patch file in the c: root folder. If you wish, you can change the output location, or omit '-o c:' to save the patch to the current folder. Send the generated patch file to [magicgrove mailing list](mailto:magicgrove@googlegroups.com) as attachment. A project maintainer will integrate your changes into master branch.

When integration is complete, you can update your master branch by typing:

```
git checkout master
git pull origin
```

You can also delete your private feature branch by typing:

```
git branch -d myNewCards
```

This concludes our first Episode. Until next time, try implementing some simple cards on your own. See you soon.

## Additional resources ##
  * To learn more about git, check out the excellent [Free Pro git book](http://git-scm.com/book).
  * Magicgrove uses xUnit for testing. To learn more about xUnit, visit [xUnit page at CodePlex](https://xunit.codeplex.com/).