// timer
int interval = 500;
int sensitivity = 50;
int lastRecordedTime = 0;
int lastRecordedTimeFollow = 0;

//positions
int curCenterX = 0;
int curCenterY = 0;
float threshold = 2;
int cellSize = 6;
float probabilityOfAliveAtStart = 0.4;
int round = 0;
int raySpeed = 10;
boolean shoot = false;

//colors
color pink = color(214, 60, 129);
color green = color(0, 200, 0);
color black = color(0);
color white = color(255, 255, 255);
color red = color(200, 0, 0);


//TODO: dot moving aiming (ok)
//TODO: lazer animation
//TODO: other decoration
int[][] cells; 
int[][] cellsBuffer; 

boolean pause = false;

void shootRay(int dir){
  //0: left-> right; 1: right->left; 2: up->down; 3: down->up
  
}

void lightRay(){
  round = (round + 1) % raySpeed;
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
      if(cells[x][y] == 2 || cells[x][y] == 3){
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

  curCenterX = constrain(mouseX / cellSize, 1, width / cellSize - 2);
  curCenterY = constrain(mouseY / cellSize, 1, height / cellSize - 2);
  if(shoot){
    shootRay(4);
  }else{
    lightRay();
  }
  cells[curCenterX][curCenterY] = 2;
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

void setup() {
  size (1600, 900);
  cells = new int[width/cellSize][height/cellSize];
  cellsBuffer = new int[width/cellSize][height/cellSize];

  stroke(48);
  noSmooth();

  fillgrid();
  background(0); 
}


void draw() {

  //Draw grid
  for (int x=0; x<width/cellSize; x++) {
    for (int y=0; y<height/cellSize; y++) {
      if(cells[x][y] == 0) fill(black);
      else if(cells[x][y]==1) fill(green);
      else if(cells[x][y] == 2) fill(pink);
      else if(cells[x][y] == 3) fill(white);
      else if(cells[x][y] == 4) fill(red);
      rect (x*cellSize, y*cellSize, cellSize, cellSize);
    }
  }
  // Iterate if timer ticks
  if (millis()-lastRecordedTime>interval) {
    if (!pause) {
      iteration();
      lastRecordedTime = millis();
    }
  }
   if (millis()-lastRecordedTimeFollow>sensitivity) {
    if (!pause) {
      follow();
      lastRecordedTimeFollow = millis();
    }
  }

  // Create  new cells manually on pause
  if (pause && mousePressed) {
    // Map and avoid out of bound errors
    int xCellOver = int(map(mouseX, 0, width, 0, width/cellSize));
    xCellOver = constrain(xCellOver, 0, width/cellSize-1);
    int yCellOver = int(map(mouseY, 0, height, 0, height/cellSize));
    yCellOver = constrain(yCellOver, 0, height/cellSize-1);

    // Check against cells in buffer
    if (cellsBuffer[xCellOver][yCellOver]==1) { // Cell is alive
      cells[xCellOver][yCellOver]=0; // Kill
      fill(black); // Fill with kill color
    }
    else { // Cell is dead
      cells[xCellOver][yCellOver]=1; // Make alive
      fill(green); // Fill alive color
    }
  } 
  else if (pause && !mousePressed) { // And then save to buffer once mouse goes up
    // Save cells to buffer (so we opeate with one array keeping the other intact)
    for (int x=0; x<width/cellSize; x++) {
      for (int y=0; y<height/cellSize; y++) {
        cellsBuffer[x][y] = cells[x][y];
      }
    }
  }
}

void iteration() { // When the clock ticks
  fillgrid();
} // End of function

void keyPressed() {
  if (key=='r' || key == 'R') {
    // Restart: reinitialization of cells
    for (int x=0; x<width/cellSize; x++) {
      for (int y=0; y<height/cellSize; y++) {
        float state = random (100);
        if (state > probabilityOfAliveAtStart) {
          state = 0;
        }
        else {
          state = 1;
        }
        cells[x][y] = int(state); // Save state of each cell
      }
    }
  }
  if (key==' ') { // On/off of pause
    pause = !pause;
  }
  if (key=='c' || key == 'C') { // Clear all
    for (int x=0; x<width/cellSize; x++) {
      for (int y=0; y<height/cellSize; y++) {
        cells[x][y] = 0; // Save all to zero
      }
    }
  }
}
