# DelayedCommands
A set of commands with built in adjustable delays for buttons that stop spam clickers



The Problem:
http://stackoverflow.com/questions/38480982/how-to-stop-users-from-spamming-buttons-in-wpf/38504618#38504618
I needed a way to stop users spamming a buton, this was because the button skipped my media element forward by 5 minutes. If you spam this button it repeats the skip which is an expensive operation. Instead i wish to add up all the times the button is spam clicked and once spamming stops execute 1 big skip operation by the amount of times the button was spammed.

To keep this reusable i decided to make my own set of commands to do this

1. Delayed Command: this command can take a time span. and adds all the times the button was clicked within the time span.

2. Delayed Command with function: This command does the same as the first but instead of containing a number of times clicked it takes an extra function at initalization so the user can inject this logic and do whatever they like upon a spam click

3. Rate Limited Command: the first commands would wait until the user has stopped spamming the button to do anything, this causes a lag effect. this solves the lag effect by running execute every second dispite the number of clicks but does not lose the actual number of clicks

any questions feel free to contact samgreaves0@gmail.com

feel free to use and alter and contribute to this git
