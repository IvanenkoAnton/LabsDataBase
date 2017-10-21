from menu import Menu


menu = Menu()
active = True

while active:
    menu.render()
    active = menu.choose()
