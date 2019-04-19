using UnityEngine;
using UnityEngine.UI;

namespace Tanks {
    public class Tempest : Tank {
        private int mineCount = 5;
        private int maxMineCount = 10;
        private float currentMineTimer = 0.0f;
        private float mineRefill = 10.0f;
        public Transform mine;

        public override void UpdateSpecialStats() {
            var canvas = transform.Find("Canvas");
            canvas.Find("Special").GetComponent<Text>().text = mineCount + "/" + maxMineCount + " (" + (mineRefill-currentMineTimer).ToString("F1") + "s)";
        }

        public override void UpdateSpecial(float dTime, GamePad.Index idx) {
            if (mineCount < maxMineCount) {
                currentMineTimer += dTime;

                if (currentMineTimer >= mineRefill) {
                    mineCount++;
                    currentMineTimer = 0.0f;
                }
            }
        }

        public override void Special() {
            if (mineCount > 0) {
                Instantiate(mine, transform.position, transform.rotation).transform.GetComponent<Mine>().Owner = padNumber;

                AudioManager.instance.PlayMinePlacedSound();
                mineCount--;
            }
        }

        public void Update() {
            base.Update();

            if (controller.SpecialDown())
                Special();
        }
    }
}