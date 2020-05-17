# Thesis_HJC885

Updates:

-Removed unused shooting features

+Added new shooting features
	-Fixed (can define a vector for projectile destination)
	-Random (generating random not duplicated vectors)

+Added new Robot Movements
	+Head movement
	+Head Rotation
	+Body movement
	+Body Rotation
	+Leg movement
	+Leg Rotation

+Added Resrictions for all bodypart
	+Rotation restrictions
	+Movement restricitons based on width,height parameters

+Added Projectile vector Constraints
	+Projectile vectors normalized
	+Projectile vector duplicate constraint

+Modified parameters for FullHD (1920x1080) sampling
	+image processing parameters adjusted
	+recalculate refining vector positions

+Added Robot Decision making feature
	+Added RobotBrain GameObject
		+Added fitness value to One movement (solution)
		+Added Evaluation: Solutions can be Evaluated
		+Uses Genetic Algorithm to find soulution
		+Calculate Optimal robot movement based on input vector
	
