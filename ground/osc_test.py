"""
from pythonosc.udp_client import SimpleUDPClient

# OSC setup
ip = "127.0.0.1"  # Server IP
port = 12000      # Server port
client = SimpleUDPClient(ip, port)

# Send Position data
def send_position(position_x, position_y):
    data = [0, float(position_x), float(position_y)]
    client.send_message("/action", data)
    print(f"Sent Position Data: {data}")

# Send Shoot data
def send_shoot(is_shooting, direction):
    data = [1, bool(is_shooting), int(direction)]
    client.send_message("/action", data)
    print(f"Sent Shoot Data: {data}")

# Main loop for user interaction
try:
    print("Press keys to send data:")
    print("[1] Position Data: Enter x (0->Left, 1->Right), y (0->Bottom, 1->Top)")
    print("[2] Shoot Data: Enter shooting (0->No, 1->Yes), direction (e.g., -1, 0, 1)")
    print("[CTRL+C] Quit the program")

    while True:
        action = input("\nChoose [1] Position or [2] Shoot: ").strip()

        if action == "1":  # Send Position Data
            position_x = input("Enter positionX (0.0->Left, 1.0->Right): ").strip()
            position_y = input("Enter positionY (0.0->Bottom, 1.0->Top): ").strip()
            send_position(position_x, position_y)

        elif action == "2":  # Send Shoot Data
            shooting = input("Enter shooting (0->No, 1->Yes): ").strip()
            direction = input("Enter direction (e.g., -1, 0, 1): ").strip()
            send_shoot(shooting, direction)

        else:
            print("Invalid choice. Try again.")

except KeyboardInterrupt:
    print("\nProgram terminated.")
"""


from pythonosc import udp_client
from time import sleep

# Set up the UDP client to send data to the Processing receiver
ip = "127.0.0.1"  # Localhost, change to the IP address of the receiver if needed
port = 12000  # Same port as in the Processing code
client = udp_client.SimpleUDPClient(ip, port)

# Function to send position data
def send_position_data():
    # Starting position for [x, y]
    start_x = 0.0
    start_y = 0.0
    step = 0.01  # Increment step
    
    # Loop through the x and y values and send OSC messages
    for x in range(101):  # For positionX from 0 to 1 (101 steps)
        position_x = x * step
        position_y = x * step
        client.send_message("/action", [0, position_x, position_y])
        # print(f"Sent: [0, {position_x}, {position_y}]")
        sleep(0.1)

def send_shooting_data():
    # Directions for shooting: 0=up, 1=left, 2=down, 3=right
    directions = [0, 1, 2, 3]

    for dir in directions:
        # Create OSC message with actionType 1 for shooting
        client.send_message("/action", [1, dir])  # Shooting
        print(f"Sent Shooting: [1, {dir}]")
        sleep(1)  # Optional: wait a bit before sending the next message


# Call the function to send the data
while(1):
    send_position_data()
    client.send_message("/action", [0, 0.5, 0.5])
    send_shooting_data()
