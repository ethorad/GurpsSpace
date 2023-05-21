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
