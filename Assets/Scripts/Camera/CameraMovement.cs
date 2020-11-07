using UnityEngine;

using Grigorov.Extensions;
using Grigorov.LeapAndJump.Level;

namespace Grigorov.LeapAndJump.CameraManagement
{
	public class CameraMovement : MonoBehaviour
	{
		[SerializeField] float     _lerp      = 3;
		[SerializeField] Vector2   _offset    = new Vector2(0, 0.1f);
		[SerializeField] Transform _edgePoint = null;

		Player _player = null;
		Camera _camera = null;

		Camera Camera => this.GetComponent(ref _camera);

		void Awake()
		{
			_player = FindObjectOfType<Player>();
		}

		void FixedUpdate()
		{
			transform.position = CalculateCameraPosition();
		}

		Vector3 CalculateCameraPosition()
		{
			var curPos = transform.position;
			curPos = Vector2.Lerp(curPos, _player.transform.position, _lerp * Time.deltaTime);
			curPos += (Vector3)_offset;
			curPos.z = -10;
			curPos.x = 0;
			curPos.y = Mathf.Clamp(curPos.y, CalculateMinYPos(), CalculateMaxYPos());
			return curPos;
		}

		float CalculateMinYPos()
		{
			var bottomLeft = Camera.ViewportToWorldPoint(new Vector2(0, 0));
			var minYPos = _edgePoint.position.y + (transform.position.y - bottomLeft.y);
			return minYPos;
		}

		float CalculateMaxYPos()
		{
			var bottomLeft = Camera.ViewportToWorldPoint(new Vector2(0, 0));
			var topLeft = Camera.ViewportToWorldPoint(new Vector2(0, 1));
			var cameraYScale = topLeft.y - bottomLeft.y;
			var maxYPos = _player.transform.position.y + (cameraYScale / 2);
			return maxYPos;
		}
	}
}