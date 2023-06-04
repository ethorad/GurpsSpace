# GURPS Space

A program to generate planets etc for a GURPS Space campaign, using the system set out in the GURPS Space book.

## Changes from the rules as written

Some bits where I've changed the creation process from the rules in the book are noted below.  These are in no particular order.

### Random settlement type
The rules say settlement types should be decided by the GM.  I've added some code so this can be determined randomly.  Roll 1d on the table below:
- 1: No settlement
- 2-3: Outpost
- 4-5: Colony
- 6: Homeworld

If the settlement is a colony, then an age is determined randomly between 1 and 200 years uniformly. This is used for the population.  If the settlement is a homeworld, then it is assumed to be interstellar on a roll of 1-5 on 1d, and uncontacted on a 6.

### Colony population
Rather than using the table in the rulebook, I've used a formula approach from the breakout box on GURPS Space page 93.  This links to attributes of the populating species.

### Limits
Some of the planet limits can be breached by user inputs.  For example, on the random table planets can only get resorce values in the range (-2,+2).  The user input screen notes this, but permits entries over the full range (-5,+5).  This could have the limit enforced, but I opted not to.  For things which are more linked to the consistency of the planet (such as density, temperature, etc) I have tended to keep the range limits.

### Trade volume
Strictly the trade volume for a planet should be calculated by reference to its various trade routes to other planets, as set out on GURPS Space page 95.  However, at this stage I haven't coded up the ability to link worlds with trade routes.  As such, user input trade volume just asks for what proportion of the total economic value to use as the trade volume.  For random trade volume I use the below to generate the proportion of total economic value:
- Uncontacted homeworld: 0% trade volume, as no contact with other planets
- Contacted homeworld: 10-40% trade volume, fairly low as expect to be reasonably sufficient so most economic activity is internal
- Colony: 30-70% trade volume, higher volume as will generally have smaller economies and be more tightly linked back to other planets of the same civilisation
- Outpost: 80-100% trade volume, very high as likely most goods will need to be brought in - and any stuff produced is likely for shipping back out

### Chance of an espionage facility or refugee camp
The rules state that an espionage facility is present on a roll of (PR+6) or less on 3d.  If one is present, keep rolling to generate more until a roll fails.  The problem with this is for large settlements with say PR 8 there is an espionage facility on a roll of 14 or less.  This can result in dozens of espionage facilities until one fails.
I've therefore changed this so that subsequent facilities have a cumulative -1 to the target number, making them successively less likely.  This puts a cap on the number of possible espionage facilities, and gives a more reasonable number overall.  I've also added a hard cap of 10 facilities.
A similar issue exists for refugee camps, whereby the rules say to keep generating them until the roll fails.  However since the target is (PR-3), even with high PR 8 say facilities, you need a roll of 5 or less which is only 4.6%.  As such you're much less likely to get multiples.  I've therefore not adjusted down the target number with successive camps.  Again I've added a hard cap of 10 facilities.

## Licencing
The code is licenced under the Creative Commons [BY-NC-SA 4.0](https://creativecommons.org/licenses/by-nc-sa/4.0/) licence.  Essentially this means:
- BY: Any reuse has to give appropriate credit to me
- NC: Reuse is permitted except for commercial purposes
- SA: Any reuse has to be shared under the same licence
