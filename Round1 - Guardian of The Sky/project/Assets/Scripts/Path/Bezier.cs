using UnityEngine;

public static class Bezier {
	// LINEAR formula
	// B(t) = (1 - t) P0 + t P1

	// QUADRATIC formula
	// B(t) = (1 - t)^2 P0 + 2 (1 - t) t P1 + t^2 P2. 
	public static Vector3 GetPoint(Vector3 p0, Vector3 p1, Vector3 p2, float t) {
		t = Mathf.Clamp01 (t);
		return (Mathf.Pow ((1f - t), 2) * p0) + (2f * (1f - t) * t * p1) + (Mathf.Pow (t, 2) * p2);
	}
	// first derivative
	// B'(t) = 2 (1 - t) (P1 - P0) + 2 t (P2 - P1)
	public static Vector3 GetFirstDerivative(Vector3 p0, Vector3 p1, Vector3 p2, float t) {
		t = Mathf.Clamp01 (t);
		return (2f * (1f - t) * (p1 - p0)) + (2f * t * (p2 - p1));
	}

	// CUBIC formula
	// B(t) = (1 - t)^3 P0 + 3 (1 - t)^2 t P1 + 3 (1 - t) t^2 P2 + t^3 P3
	public static Vector3 GetPoint(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t) {
		t = Mathf.Clamp01 (t);
		return (Mathf.Pow ((1f - t), 3) * p0) + (3f * Mathf.Pow ((1f - t), 2) * t * p1) + (3f * (1f - t) * Mathf.Pow (t, 2) * p2) + (Mathf.Pow (t, 3) * p3);
	}
	// first derivative
	// B'(t) = 3 (1 - t)^2 (P1 - P0) + 6 (1 - t) t (P2 - P1) + 3 t^2 (P3 - P2)
	public static Vector3 GetFirstDerivative(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t) {
		t = Mathf.Clamp01 (t);
		return (3f * Mathf.Pow ((1f - t), 2) * (p1 - p0)) + (6f * (1f - t) * t * (p2 - p1)) + (3f * Mathf.Pow (t, 2) * (p3 - p2));
	}
}
