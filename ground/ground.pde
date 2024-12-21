import oscP5.*;
import netP5.*;

OscP5 oscP5;
NetAddress myRemoteLocation;

// timer
// TODO: align two direction speed
int interval = 300;
int sensitivity = 50;
int lastRecordedTime = 0;
int lastRecordedTimeFollow = 0;

//positions
int gridWidth = 1920;
int gridHeight = 1080;
int widthCellRatio = 320;
int heightCellRatio = 180;
float threshold = 2;
int cellSize = 6;
float probabilityOfAliveAtStart = 0.4;
int round = 0;
int roundShooting = 0;
static int raySpeed = 10;
static int shootingSpeedHor = 40;
static int shootingSpeedVer = (int)(40.0 / 16.0 * 9.0);
int shootingTimer = width/cellSize;
//received data
int curCenterX;
int curCenterY;
boolean shoot = false;
int receivedDir = -1;

//colors
color pink = color(214, 60, 129);
color green = color(0, 200, 0);
color black = color(0);
color white = color(255, 255, 255);
color red = color(255, 0, 0);

//decorations
ImageObject palmTreeImg, flowerImg, rabbitImg;

// Rabbit jumping parameters
float jumpTime = 0; // current time
float jumpDuration = 60; // frame num
float jumpHeight = 150; // max height
int rabbitHorizontal;
int rabbitVertical;

class ImageObject{
  PImage img;
  int imgWidth;
  int imgHeight;
  
  ImageObject(PImage img, int imgWidth, int imgHeight) {
    this.img = img;
    this.imgWidth = imgWidth;
    this.imgHeight = imgHeight;
  }
}

int[][] cells; 
int[][] cellsBuffer; 

boolean pause = false;

void shootRay(int dir){
  if(dir == -1) return;
  println("shoot ray");
  //0: left-> right; 1: right->left; 2: up->down; 3: down->up
  //updated: 0 for upward, 1 for leftward, 2 for downward, 3 for rightward
  if(dir == 1 || dir == 3) roundShooting += shootingSpeedHor;
  else roundShooting += shootingSpeedVer;
  //println(roundShooting);
  //if(roundShooting >= width/cellSize) roundShooting = 0;
  for (int x=0; x<width/cellSize; x++) {
    for (int y=0; y<height/cellSize; y++) {
      if(dir == 1){
        if(abs(y - curCenterY) < threshold && x < roundShooting){
          cells[x][y] = 4;
        }
      } else if(dir == 3){
        if(abs(y - curCenterY) < threshold && (width/cellSize - x) < roundShooting){
          cells[x][y] = 4;
        }
      } else if(dir == 0){
        if(abs(x - curCenterX) < threshold && y < roundShooting){
          cells[x][y] = 4;
        }
      }else if(dir == 2){
        if(abs(x - curCenterX) < threshold && (height/cellSize - y) < roundShooting){
          cells[x][y] = 4;
        }
      }
    }
  }
}

void lightRay(){
  round++;
  for (int x=0; x<width/cellSize; x++) {
    for (int y=0; y<height/cellSize; y++) {
      if(abs(x - curCenterX) < threshold && (abs(y - curCenterY) + round) % raySpeed > 2){
        cells[x][y] = 3;
      }
      else if(abs(y - curCenterY) < threshold && (abs(x - curCenterX) + round) % raySpeed > 2){
        cells[x][y] = 3;
      }
    }
  }
}

void follow(){
  int radius = 7; // Radius of the circle in cells
  for (int x = 0; x < width / cellSize; x++) {
    for (int y = 0; y < height / cellSize; y++) {
      if(cells[x][y] == 2 || cells[x][y] == 3 || cells[x][y] == 4){
        float state = random (100);
        if (state > probabilityOfAliveAtStart) { 
          cells[x][y] = 0;
        }
        else {
         cells[x][y] = 1;
        }
      }
    }
  }

  //curCenterX = constrain(mouseX / cellSize, 1, width / cellSize - 2); //TO RECEIVE
  //curCenterY = constrain(mouseY / cellSize, 1, height / cellSize - 2); //TO RECIEVE
  curCenterX = constrain(curCenterX, 1, width / cellSize - 2); //TO RECEIVE
  curCenterY = constrain(curCenterY, 1, height / cellSize - 2); //TO RECIEVE
  lightRay();
  if(shoot){ //shoot time and direction; TO RECEIVE
    shootingTimer = 0;
    roundShooting = 0;
    shoot = false;
    //curDir = (int)random(4);
  }
  int shootingTimerThreshold;
  shootingTimerThreshold = width/cellSize/shootingSpeedHor;
  if(shootingTimer < shootingTimerThreshold){
    shootRay(receivedDir);
    shootingTimer++;
    //roundShooting++;
    if(shootingTimer == shootingTimerThreshold){
      receivedDir = -1;
      shoot = false;
    }
  }
  //drawing center
  cells[curCenterX][curCenterY] = 2;
  //drawing target sign
  for (int x = 0; x < width / cellSize; x++) {
    for (int y = 0; y < height / cellSize; y++) {
      int dx = x - curCenterX;
      int dy = y - curCenterY;
      if (dx * dx + dy * dy >= (radius - 1) * (radius - 1) && dx * dx + dy * dy <= radius * radius) {
        cells[x][y] = 2;
      }else if((dx == 0 && abs(dy) > 2 && abs(dy) < 10) || (dy == 0 && abs(dx) > 2 && abs(dx) < 10)){
        cells[x][y] = 2;
      }
    }
  }
}


void fillgrid(){
  for (int x=0; x<width/cellSize; x++) {
    for (int y=0; y<height/cellSize; y++) {
      if(cells[x][y] == 2) continue;
      else if(cells[x][y] == 1){
        boolean valid = false;
        int failCnt = 0;
        int[] dx = {-1, 0, 1, 0};
        int[] dy = {0, -1, 0, 1};
        while(!valid){
          failCnt++;
          int dir = (int)random(4);
          int nx = x + dx[dir];
          int ny = y + dy[dir];
          if(nx >= 0 && nx < width/cellSize && ny >= 0 && ny < height/cellSize){
            valid = true;
            cells[x][y] = 0;
            cells[nx][ny] = 1;
          }
          if(failCnt == 10) break;
        }
      }
    }
  }
}

ImageObject createImageObj(String filename, float scale){
  PImage img = loadImage(filename); 
  int imgHeight = int(scale * height);
  float aspectRatio = float(img.width) / float(img.height);
  int imgWidth = int(imgHeight * aspectRatio);
  img.resize(imgWidth, imgHeight); 
  return new ImageObject(img, imgWidth, imgHeight);
}

void oscEvent(OscMessage theOscMessage){
  if (theOscMessage.checkAddrPattern("/action")) {
    // Read the type of action (first element in the message)
    int actionType = theOscMessage.get(0).intValue();
    
    if (actionType == 0) {  // Position data
      float positionX = theOscMessage.get(1).floatValue();
      float positionY = theOscMessage.get(2).floatValue();
      println("Received Position Data: PositionX=" + positionX + ", PositionY=" + positionY);
      curCenterX = (int)(width * positionX) / cellSize;
      curCenterY = (int)(height * (1 - positionY)) / cellSize;
      println(curCenterX, curCenterY);
    }
    else if (actionType == 1) {  // Shoot data
      shoot = true;
      receivedDir = theOscMessage.get(1).intValue();
      println("Received Shoot Data: Direction=" + receivedDir);
    }
    else {
      println("Unknown action received.");
    }
  }
}
void setup() {
  //size(gridWidth, gridHeight);
  size(1920, 1080); //cell: (320, 180)
  cellSize = gridWidth / widthCellRatio;
  println(cellSize);
  curCenterX = 0;
  curCenterY = gridHeight / cellSize;

  //osc related
  oscP5 = new OscP5(this, 12000);
  myRemoteLocation = new NetAddress("127.0.0.1",12000);
  
  cells = new int[width/cellSize][height/cellSize];
  cellsBuffer = new int[width/cellSize][height/cellSize];

  palmTreeImg = createImageObj("palm_tree_2.png", 0.09); 
  flowerImg = createImageObj("flowers.png", 0.04);
  rabbitImg = createImageObj("rabbit.png", 0.03);
  rabbitHorizontal = (int)(0.1 * flowerImg.imgWidth * cellSize) + (int)(2 * rabbitImg.imgWidth * cellSize);
  rabbitVertical = height - (int)(rabbitImg.imgHeight);

  stroke(48);
  noSmooth();

  //fillgrid();
  for (int x=0; x<width/cellSize; x++) {
    for (int y=0; y<height/cellSize; y++) {
      float state = random (100);
      if (state > probabilityOfAliveAtStart) { 
        cells[x][y] = 0;
      }
      else {
        cells[x][y] = 1;
      }
    }
  }
  background(0); 
}

void drawImage(PImage img, int startX, int startY) {
  for (int x = 0; x < img.width; x++) {
    for (int y = 0; y < img.height; y++) {
      color c = img.get(x, y);
      if (alpha(c) > 0) { //transparency
        fill(c);
        noStroke();
        rect(startX + x * cellSize, startY + y * cellSize, cellSize, cellSize);
      }
    }
  }
}

void draw() {

  for (int x=0; x<width/cellSize; x++) {
    for (int y=0; y<height/cellSize; y++) {
      if(cells[x][y] == 0) fill(black);
      else if(cells[x][y] == 1) fill(green);
      else if(cells[x][y] == 2) fill(pink);
      else if(cells[x][y] == 3) fill(white);
      else if(cells[x][y] == 4) fill(red);
      rect (x*cellSize, y*cellSize, cellSize, cellSize);
    }
  }
  drawImage(palmTreeImg.img, width - (int)(1.1 * palmTreeImg.imgWidth * cellSize), height - palmTreeImg.imgHeight * cellSize);
  drawImage(flowerImg.img, (int)(0.1 * flowerImg.imgWidth * cellSize), height - (int)(1.1 * flowerImg.imgHeight * cellSize));

  jumpTime++;
  float jumpY = jumpHeight * (4 * pow(jumpTime / jumpDuration - 0.5, 2) + 1); //parabola for jumping path
  rabbitVertical = height - (int)(rabbitImg.imgHeight) - (int)jumpY;

  drawImage(rabbitImg.img, rabbitHorizontal, rabbitVertical);

  if(jumpTime >= jumpDuration) {
    jumpTime = 0;//reset
    rabbitHorizontal = (int)random(width - 1.2 * rabbitImg.imgWidth);
  }

  if(millis()-lastRecordedTimeFollow>sensitivity) {
    if(!pause) {
      follow();
      lastRecordedTimeFollow = millis();
    }
  }else if(millis()-lastRecordedTime>interval) {
    if(!pause) {
      iteration();
      lastRecordedTime = millis();
    }
  }
}

void iteration() { // When the clock ticks
  fillgrid();
} // End of function
